//jqgrid 扩展
(function ($) {
    $.jgrid.extend({
        addRow: function (ret, pos, callback) {
            var rdata = {};
            if (ret.error) {
                $.gritter.add({ text: 'warn: ' + ret.error });
                return false;
            } else if (ret.value) {
                rdata = ret.value;
            }
            //grid.get(0).p.savedRow.length
            var $t = this.get(0);
            if ($t.p.savedRow.length > 0) {
                // save the cell
                if (this.jqGrid("saveCell", $t.p.savedRow[0].id, $t.p.savedRow[0].ic) === false) {
                    //focus
                    return -1;
                }
            }
            var rowid = this.getNextId();
            if (rdata) {
                rdata.id = rowid;
                if (this.get(0).p.treeGrid) {
                    var pid = this.getGridParam("selrow");
                    this.addChildNode(rowid, pid, rdata);
                } else {
                    this.addRowData(rowid, rdata, pos || "last"); //last first
                    this.setSelection(rowid);
                }
                if ($.type(callback) == "function") callback(rdata, rowid);
                return this.getGridParam("reccount");
            }
            else {
                return -1;
            }
        }
     , delSelectedRow: function (func) {
         var gird = this;
         var selectedId = gird.getGridParam("selrow");
         if (!selectedId) {
             $.gritter.add({ text: 'Please select the record to delete!' });
             return;
         }
         swal({
             title: "", text: "Are you sure you want to delete the selected record?", type: "warning", showCancelButton: true,
         }, function (isConfirm) {
             if (!isConfirm) return false;
             var rowData = gird.getRowData(selectedId);
             func(rowData, selectedId, function (ret) {
                 if (ret.error) {
                     $.gritter.add({ text: 'warn: ' + ret.error });
                     return false;
                 } else if (ret.value) {
                     gird.delRowData(selectedId);
                     $.gritter.add({ text: 'success!' });
                     return true;
                 } else {
                     $.gritter.add({ text: 'failed!' });
                     return false;
                 }
             });
         });
     }
     , editSelectedRow: function (func) {
         var selectedId = this.getGridParam("selrow");
         window.jqGridSelectedId = selectedId;
         if (!selectedId) {
             $.gritter.add({ text: 'Please select the records you want to edit!' });
             return;
         }
         var rowData = this.getRowData(selectedId);
         func(rowData, function (ret) {
             var rdata = ret && ret.value ? JSON.parseJson(ret.value) : ret;
             if (rdata) {
                 this.setRowData(selectedId, rdata) //, cssp
             }
         });
     }
     , getNextId: function () {
         var ids = this.getDataIDs();
         var rowid = ids.length == 0 ? 0 : (Number(Math.max.apply(Math, ids)) + 1);
         return rowid || 0;
     }
     , getRowId: function (iRow) {
         var r = this.get(0).rows[iRow];
         if (r) {
             return r.id;
         }
         else
             return null;
     }
     , getSelectedRowData() {
         var selectedId =this.getGridParam("selrow");
         var rowData = this.getRowData(selectedId);
         return rowData;
     }
     , initSelect: function (id, data, value, changeFunc) {
         var ipt = $("#" + id).hide();
         var selId = id + "_sel";
         var sel = $("#" + selId);
         if (sel.size() == 0) {
             var selHtml = "<select id='" + selId + "'>";
             for (var i = 0; i < data.length; i++) {
                 selHtml += "<option value='" + data[i]["value"] + "'>" + data[i]["text"] + "</option>";
             }
             selHtml += "</select>";
             ipt.after($(selHtml));
             sel = $("#" + selId).val(value).bind('change', function () {
                 var txt = sel.find("option:selected").text();
                 ipt.val(txt);
                 changeFunc.call(this, this.value, txt);
             });
         }
         sel.val(value);
     }, endEdit: function () {
         //取消当前处于编辑状态的单元格状态便于取值（正在编辑的单元格，取到的的值是html元素）
         var $t = this.get(0);
         if ($t.p) {
             if (!$t.p.knv) { $($t).jqGrid("GridNav"); }
             if ($t.p.savedRow.length > 0) {
                 for (var i = 0; i < $t.p.savedRow.length; i++) {
                     $($t).jqGrid("saveCell", $t.p.savedRow[i].id, $t.p.savedRow[i].ic);//saveCell避免当前编辑的单元格值丢失
                 }
             }
         }
     }
     , merger: function (cellName) {
         //得到显示到界面的id集合
         var mya = this.getDataIDs();
         //当前显示多少条
         var length = mya.length;
         for (var i = 0; i < length; i++) {
             //从上到下获取一条信息
             var before = this.getRowData(mya[i]);
             //定义合并行数
             var rowSpanTaxCount = 1;
             for (j = i + 1; j <= length; j++) {
                 //和上边的信息对比 如果值一样就合并行数+1 然后设置rowspan 让当前单元格隐藏
                 var end = this.getRowData(mya[j]);
                 if (before[cellName] == end[cellName]) {
                     rowSpanTaxCount++;
                     this.setCell(mya[j], cellName, '', { display: 'none' });
                 } else {
                     rowSpanTaxCount = 1;
                     break;
                 }
                 $("#" + cellName + "" + mya[i] + "").attr("rowspan", rowSpanTaxCount);
             }
         }
     }
    });

})(jQuery);

function jqgrid_fill(selector) {
    setTimeout(function () {
        var _h = $("#navbar").height() + $("#breadcrumbs").height() + $("#toolbar").height();
        $(selector).jqGrid('setGridWidth', $("#page-content").width()).jqGrid('setGridHeight', document.documentElement.clientHeight - _h - 120);
    }, 100);
}