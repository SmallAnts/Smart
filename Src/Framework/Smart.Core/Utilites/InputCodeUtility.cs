using System;
using System.Text;
using System.Text.RegularExpressions;

namespace Smart.Core.Utilites
{
    /// <summary>
    /// 用户输入码工具类
    /// </summary>
    internal static class InputCodeUtility
    {
        // Fields
        private const int firstChCode = -20319;
        private const int lastChCode = -2050;
        private const int lastOfOneLevelChCode = -10247;
        private static Regex regex = new Regex("[一-龥]$");

        #region otherChinese
        private static string[] otherChinese = new string[] {
        "亍", "丌", "兀", "丐", "廿", "卅", "丕", "亘", "丞", "鬲", "孬", "噩", "丨", "禺", "丿", "匕",
        "乇", "夭", "爻", "卮", "氐", "囟", "胤", "馗", "毓", "睾", "鼗", "丶", "亟", "鼐", "乜", "乩",
        "亓", "芈", "孛", "啬", "嘏", "仄", "厍", "厝", "厣", "厥", "厮", "靥", "赝", "匚", "叵", "匦",
        "匮", "匾", "赜", "卦", "卣", "刂", "刈", "刎", "刭", "刳", "刿", "剀", "剌", "剞", "剡", "剜",
        "蒯", "剽", "劂", "劁", "劐", "劓", "冂", "罔", "亻", "仃", "仉", "仂", "仨", "仡", "仫", "仞",
        "伛", "仳", "伢", "佤", "仵", "伥", "伧", "伉", "伫", "佞", "佧", "攸", "佚", "佝", "佟", "佗",
        "伲", "伽", "佶", "佴", "侑", "侉", "侃", "侏", "佾", "佻", "侪", "佼", "侬", "侔", "俦", "俨",
        "俪", "俅", "俚", "俣", "俜", "俑", "俟", "俸", "倩", "偌", "俳", "倬", "倏", "倮", "倭", "俾",
        "倜", "倌", "倥", "倨", "偾", "偃", "偕", "偈", "偎", "偬", "偻", "傥", "傧", "傩", "傺", "僖",
        "儆", "僭", "僬", "僦", "僮", "儇", "儋", "仝", "氽", "佘", "佥", "俎", "龠", "汆", "籴", "兮",
        "巽", "黉", "馘", "冁", "夔", "勹", "匍", "訇", "匐", "凫", "夙", "兕", "亠", "兖", "亳", "衮",
        "袤", "亵", "脔", "裒", "禀", "嬴", "蠃", "羸", "冫", "冱", "冽", "冼", "凇", "冖", "冢", "冥",
        "讠", "讦", "讧", "讪", "讴", "讵", "讷", "诂", "诃", "诋", "诏", "诎", "诒", "诓", "诔", "诖",
        "诘", "诙", "诜", "诟", "诠", "诤", "诨", "诩", "诮", "诰", "诳", "诶", "诹", "诼", "诿", "谀",
        "谂", "谄", "谇", "谌", "谏", "谑", "谒", "谔", "谕", "谖", "谙", "谛", "谘", "谝", "谟", "谠",
        "谡", "谥", "谧", "谪", "谫", "谮", "谯", "谲", "谳", "谵", "谶", "卩", "卺", "阝", "阢", "阡",
        "阱", "阪", "阽", "阼", "陂", "陉", "陔", "陟", "陧", "陬", "陲", "陴", "隈", "隍", "隗", "隰",
        "邗", "邛", "邝", "邙", "邬", "邡", "邴", "邳", "邶", "邺", "邸", "邰", "郏", "郅", "邾", "郐",
        "郄", "郇", "郓", "郦", "郢", "郜", "郗", "郛", "郫", "郯", "郾", "鄄", "鄢", "鄞", "鄣", "鄱",
        "鄯", "鄹", "酃", "酆", "刍", "奂", "劢", "劬", "劭", "劾", "哿", "勐", "勖", "勰", "叟", "燮",
        "矍", "廴", "凵", "凼", "鬯", "厶", "弁", "畚", "巯", "坌", "垩", "垡", "塾", "墼", "壅", "壑",
        "圩", "圬", "圪", "圳", "圹", "圮", "圯", "坜", "圻", "坂", "坩", "垅", "坫", "垆", "坼", "坻",
        "坨", "坭", "坶", "坳", "垭", "垤", "垌", "垲", "埏", "垧", "垴", "垓", "垠", "埕", "埘", "埚",
        "埙", "埒", "垸", "埴", "埯", "埸", "埤", "埝", "堋", "堍", "埽", "埭", "堀", "堞", "堙", "塄",
        "堠", "塥", "塬", "墁", "墉", "墚", "墀", "馨", "鼙", "懿", "艹", "艽", "艿", "芏", "芊", "芨",
        "芄", "芎", "芑", "芗", "芙", "芫", "芸", "芾", "芰", "苈", "苊", "苣", "芘", "芷", "芮", "苋",
        "苌", "苁", "芩", "芴", "芡", "芪", "芟", "苄", "苎", "芤", "苡", "茉", "苷", "苤", "茏", "茇",
        "苜", "苴", "苒", "苘", "茌", "苻", "苓", "茑", "茚", "茆", "茔", "茕", "苠", "苕", "茜", "荑",
        "荛", "荜", "茈", "莒", "茼", "茴", "茱", "莛", "荞", "茯", "荏", "荇", "荃", "荟", "荀", "茗",
        "荠", "茭", "茺", "茳", "荦", "荥", "荨", "茛", "荩", "荬", "荪", "荭", "荮", "莰", "荸", "莳",
        "莴", "莠", "莪", "莓", "莜", "莅", "荼", "莶", "莩", "荽", "莸", "荻", "莘", "莞", "莨", "莺",
        "莼", "菁", "萁", "菥", "菘", "堇", "萘", "萋", "菝", "菽", "菖", "萜", "萸", "萑", "萆", "菔",
        "菟", "萏", "萃", "菸", "菹", "菪", "菅", "菀", "萦", "菰", "菡", "葜", "葑", "葚", "葙", "葳",
        "蒇", "蒈", "葺", "蒉", "葸", "萼", "葆", "葩", "葶", "蒌", "蒎", "萱", "葭", "蓁", "蓍", "蓐",
        "蓦", "蒽", "蓓", "蓊", "蒿", "蒺", "蓠", "蒡", "蒹", "蒴", "蒗", "蓥", "蓣", "蔌", "甍", "蔸",
        "蓰", "蔹", "蔟", "蔺", "蕖", "蔻", "蓿", "蓼", "蕙", "蕈", "蕨", "蕤", "蕞", "蕺", "瞢", "蕃",
        "蕲", "蕻", "薤", "薨", "薇", "薏", "蕹", "薮", "薜", "薅", "薹", "薷", "薰", "藓", "藁", "藜",
        "藿", "蘧", "蘅", "蘩", "蘖", "蘼", "廾", "弈", "夼", "奁", "耷", "奕", "奚", "奘", "匏", "尢",
        "尥", "尬", "尴", "扌", "扪", "抟", "抻", "拊", "拚", "拗", "拮", "挢", "拶", "挹", "捋", "捃",
        "掭", "揶", "捱", "捺", "掎", "掴", "捭", "掬", "掊", "捩", "掮", "掼", "揲", "揸", "揠", "揿",
        "揄", "揞", "揎", "摒", "揆", "掾", "摅", "摁", "搋", "搛", "搠", "搌", "搦", "搡", "摞", "撄",
        "摭", "撖", "摺", "撷", "撸", "撙", "撺", "擀", "擐", "擗", "擤", "擢", "攉", "攥", "攮", "弋",
        "忒", "甙", "弑", "卟", "叱", "叽", "叩", "叨", "叻", "吒", "吖", "吆", "呋", "呒", "呓", "呔",
        "呖", "呃", "吡", "呗", "呙", "吣", "吲", "咂", "咔", "呷", "呱", "呤", "咚", "咛", "咄", "呶",
        "呦", "咝", "哐", "咭", "哂", "咴", "哒", "咧", "咦", "哓", "哔", "呲", "咣", "哕", "咻", "咿",
        "哌", "哙", "哚", "哜", "咩", "咪", "咤", "哝", "哏", "哞", "唛", "哧", "唠", "哽", "唔", "哳",
        "唢", "唣", "唏", "唑", "唧", "唪", "啧", "喏", "喵", "啉", "啭", "啁", "啕", "唿", "啐", "唼",
        "唷", "啖", "啵", "啶", "啷", "唳", "唰", "啜", "喋", "嗒", "喃", "喱", "喹", "喈", "喁", "喟",
        "啾", "嗖", "喑", "啻", "嗟", "喽", "喾", "喔", "喙", "嗪", "嗷", "嗉", "嘟", "嗑", "嗫", "嗬",
        "嗔", "嗦", "嗝", "嗄", "嗯", "嗥", "嗲", "嗳", "嗌", "嗍", "嗨", "嗵", "嗤", "辔", "嘞", "嘈",
        "嘌", "嘁", "嘤", "嘣", "嗾", "嘀", "嘧", "嘭", "噘", "嘹", "噗", "嘬", "噍", "噢", "噙", "噜",
        "噌", "噔", "嚆", "噤", "噱", "噫", "噻", "噼", "嚅", "嚓", "嚯", "囔", "囗", "囝", "囡", "囵",
        "囫", "囹", "囿", "圄", "圊", "圉", "圜", "帏", "帙", "帔", "帑", "帱", "帻", "帼", "帷", "幄",
        "幔", "幛", "幞", "幡", "岌", "屺", "岍", "岐", "岖", "岈", "岘", "岙", "岑", "岚", "岜", "岵",
        "岢", "岽", "岬", "岫", "岱", "岣", "峁", "岷", "峄", "峒", "峤", "峋", "峥", "崂", "崃", "崧",
        "崦", "崮", "崤", "崞", "崆", "崛", "嵘", "崾", "崴", "崽", "嵬", "嵛", "嵯", "嵝", "嵫", "嵋",
        "嵊", "嵩", "嵴", "嶂", "嶙", "嶝", "豳", "嶷", "巅", "彳", "彷", "徂", "徇", "徉", "後", "徕",
        "徙", "徜", "徨", "徭", "徵", "徼", "衢", "彡", "犭", "犰", "犴", "犷", "犸", "狃", "狁", "狎",
        "狍", "狒", "狨", "狯", "狩", "狲", "狴", "狷", "猁", "狳", "猃", "狺", "狻", "猗", "猓", "猡",
        "猊", "猞", "猝", "猕", "猢", "猹", "猥", "猬", "猸", "猱", "獐", "獍", "獗", "獠", "獬", "獯",
        "獾", "舛", "夥", "飧", "夤", "夂", "饣", "饧", "饨", "饩", "饪", "饫", "饬", "饴", "饷", "饽",
        "馀", "馄", "馇", "馊", "馍", "馐", "馑", "馓", "馔", "馕", "庀", "庑", "庋", "庖", "庥", "庠",
        "庹", "庵", "庾", "庳", "赓", "廒", "廑", "廛", "廨", "廪", "膺", "忄", "忉", "忖", "忏", "怃",
        "忮", "怄", "忡", "忤", "忾", "怅", "怆", "忪", "忭", "忸", "怙", "怵", "怦", "怛", "怏", "怍",
        "怩", "怫", "怊", "怿", "怡", "恸", "恹", "恻", "恺", "恂", "恪", "恽", "悖", "悚", "悭", "悝",
        "悃", "悒", "悌", "悛", "惬", "悻", "悱", "惝", "惘", "惆", "惚", "悴", "愠", "愦", "愕", "愣",
        "惴", "愀", "愎", "愫", "慊", "慵", "憬", "憔", "憧", "憷", "懔", "懵", "忝", "隳", "闩", "闫",
        "闱", "闳", "闵", "闶", "闼", "闾", "阃", "阄", "阆", "阈", "阊", "阋", "阌", "阍", "阏", "阒",
        "阕", "阖", "阗", "阙", "阚", "丬", "爿", "戕", "氵", "汔", "汜", "汊", "沣", "沅", "沐", "沔",
        "沌", "汨", "汩", "汴", "汶", "沆", "沩", "泐", "泔", "沭", "泷", "泸", "泱", "泗", "沲", "泠",
        "泖", "泺", "泫", "泮", "沱", "泓", "泯", "泾", "洹", "洧", "洌", "浃", "浈", "洇", "洄", "洙",
        "洎", "洫", "浍", "洮", "洵", "洚", "浏", "浒", "浔", "洳", "涑", "浯", "涞", "涠", "浞", "涓",
        "涔", "浜", "浠", "浼", "浣", "渚", "淇", "淅", "淞", "渎", "涿", "淠", "渑", "淦", "淝", "淙",
        "渖", "涫", "渌", "涮", "渫", "湮", "湎", "湫", "溲", "湟", "溆", "湓", "湔", "渲", "渥", "湄",
        "滟", "溱", "溘", "滠", "漭", "滢", "溥", "溧", "溽", "溻", "溷", "滗", "溴", "滏", "溏", "滂",
        "溟", "潢", "潆", "潇", "漤", "漕", "滹", "漯", "漶", "潋", "潴", "漪", "漉", "漩", "澉", "澍",
        "澌", "潸", "潲", "潼", "潺", "濑", "濉", "澧", "澹", "澶", "濂", "濡", "濮", "濞", "濠", "濯",
        "瀚", "瀣", "瀛", "瀹", "瀵", "灏", "灞", "宀", "宄", "宕", "宓", "宥", "宸", "甯", "骞", "搴",
        "寤", "寮", "褰", "寰", "蹇", "謇", "辶", "迓", "迕", "迥", "迮", "迤", "迩", "迦", "迳", "迨",
        "逅", "逄", "逋", "逦", "逑", "逍", "逖", "逡", "逵", "逶", "逭", "逯", "遄", "遑", "遒", "遐",
        "遨", "遘", "遢", "遛", "暹", "遴", "遽", "邂", "邈", "邃", "邋", "彐", "彗", "彖", "彘", "尻",
        "咫", "屐", "屙", "孱", "屣", "屦", "羼", "弪", "弩", "弭", "艴", "弼", "鬻", "屮", "妁", "妃",
        "妍", "妩", "妪", "妣", "妗", "姊", "妫", "妞", "妤", "姒", "妲", "妯", "姗", "妾", "娅", "娆",
        "姝", "娈", "姣", "姘", "姹", "娌", "娉", "娲", "娴", "娑", "娣", "娓", "婀", "婧", "婊", "婕",
        "娼", "婢", "婵", "胬", "媪", "媛", "婷", "婺", "媾", "嫫", "媲", "嫒", "嫔", "媸", "嫠", "嫣",
        "嫱", "嫖", "嫦", "嫘", "嫜", "嬉", "嬗", "嬖", "嬲", "嬷", "孀", "尕", "尜", "孚", "孥", "孳",
        "孑", "孓", "孢", "驵", "驷", "驸", "驺", "驿", "驽", "骀", "骁", "骅", "骈", "骊", "骐", "骒",
        "骓", "骖", "骘", "骛", "骜", "骝", "骟", "骠", "骢", "骣", "骥", "骧", "纟", "纡", "纣", "纥",
        "纨", "纩", "纭", "纰", "纾", "绀", "绁", "绂", "绉", "绋", "绌", "绐", "绔", "绗", "绛", "绠",
        "绡", "绨", "绫", "绮", "绯", "绱", "绲", "缍", "绶", "绺", "绻", "绾", "缁", "缂", "缃", "缇",
        "缈", "缋", "缌", "缏", "缑", "缒", "缗", "缙", "缜", "缛", "缟", "缡", "缢", "缣", "缤", "缥",
        "缦", "缧", "缪", "缫", "缬", "缭", "缯", "缰", "缱", "缲", "缳", "缵", "幺", "畿", "巛", "甾",
        "邕", "玎", "玑", "玮", "玢", "玟", "珏", "珂", "珑", "玷", "玳", "珀", "珉", "珈", "珥", "珙",
        "顼", "琊", "珩", "珧", "珞", "玺", "珲", "琏", "琪", "瑛", "琦", "琥", "琨", "琰", "琮", "琬",
        "琛", "琚", "瑁", "瑜", "瑗", "瑕", "瑙", "瑷", "瑭", "瑾", "璜", "璎", "璀", "璁", "璇", "璋",
        "璞", "璨", "璩", "璐", "璧", "瓒", "璺", "韪", "韫", "韬", "杌", "杓", "杞", "杈", "杩", "枥",
        "枇", "杪", "杳", "枘", "枧", "杵", "枨", "枞", "枭", "枋", "杷", "杼", "柰", "栉", "柘", "栊",
        "柩", "枰", "栌", "柙", "枵", "柚", "枳", "柝", "栀", "柃", "枸", "柢", "栎", "柁", "柽", "栲",
        "栳", "桠", "桡", "桎", "桢", "桄", "桤", "梃", "栝", "桕", "桦", "桁", "桧", "桀", "栾", "桊",
        "桉", "栩", "梵", "梏", "桴", "桷", "梓", "桫", "棂", "楮", "棼", "椟", "椠", "棹", "椤", "棰",
        "椋", "椁", "楗", "棣", "椐", "楱", "椹", "楠", "楂", "楝", "榄", "楫", "榀", "榘", "楸", "椴",
        "槌", "榇", "榈", "槎", "榉", "楦", "楣", "楹", "榛", "榧", "榻", "榫", "榭", "槔", "榱", "槁",
        "槊", "槟", "榕", "槠", "榍", "槿", "樯", "槭", "樗", "樘", "橥", "槲", "橄", "樾", "檠", "橐",
        "橛", "樵", "檎", "橹", "樽", "樨", "橘", "橼", "檑", "檐", "檩", "檗", "檫", "猷", "獒", "殁",
        "殂", "殇", "殄", "殒", "殓", "殍", "殚", "殛", "殡", "殪", "轫", "轭", "轱", "轲", "轳", "轵",
        "轶", "轸", "轷", "轹", "轺", "轼", "轾", "辁", "辂", "辄", "辇", "辋", "辍", "辎", "辏", "辘",
        "辚", "軎", "戋", "戗", "戛", "戟", "戢", "戡", "戥", "戤", "戬", "臧", "瓯", "瓴", "瓿", "甏",
        "甑", "甓", "攴", "旮", "旯", "旰", "昊", "昙", "杲", "昃", "昕", "昀", "炅", "曷", "昝", "昴",
        "昱", "昶", "昵", "耆", "晟", "晔", "晁", "晏", "晖", "晡", "晗", "晷", "暄", "暌", "暧", "暝",
        "暾", "曛", "曜", "曦", "曩", "贲", "贳", "贶", "贻", "贽", "赀", "赅", "赆", "赈", "赉", "赇",
        "赍", "赕", "赙", "觇", "觊", "觋", "觌", "觎", "觏", "觐", "觑", "牮", "犟", "牝", "牦", "牯",
        "牾", "牿", "犄", "犋", "犍", "犏", "犒", "挈", "挲", "掰", "搿", "擘", "耄", "毪", "毳", "毽",
        "毵", "毹", "氅", "氇", "氆", "氍", "氕", "氘", "氙", "氚", "氡", "氩", "氤", "氪", "氲", "攵",
        "敕", "敫", "牍", "牒", "牖", "爰", "虢", "刖", "肟", "肜", "肓", "肼", "朊", "肽", "肱", "肫",
        "肭", "肴", "肷", "胧", "胨", "胩", "胪", "胛", "胂", "胄", "胙", "胍", "胗", "朐", "胝", "胫",
        "胱", "胴", "胭", "脍", "脎", "胲", "胼", "朕", "脒", "豚", "脶", "脞", "脬", "脘", "脲", "腈",
        "腌", "腓", "腴", "腙", "腚", "腱", "腠", "腩", "腼", "腽", "腭", "腧", "塍", "媵", "膈", "膂",
        "膑", "滕", "膣", "膪", "臌", "朦", "臊", "膻", "臁", "膦", "欤", "欷", "欹", "歃", "歆", "歙",
        "飑", "飒", "飓", "飕", "飙", "飚", "殳", "彀", "毂", "觳", "斐", "齑", "斓", "於", "旆", "旄",
        "旃", "旌", "旎", "旒", "旖", "炀", "炜", "炖", "炝", "炻", "烀", "炷", "炫", "炱", "烨", "烊",
        "焐", "焓", "焖", "焯", "焱", "煳", "煜", "煨", "煅", "煲", "煊", "煸", "煺", "熘", "熳", "熵",
        "熨", "熠", "燠", "燔", "燧", "燹", "爝", "爨", "灬", "焘", "煦", "熹", "戾", "戽", "扃", "扈",
        "扉", "礻", "祀", "祆", "祉", "祛", "祜", "祓", "祚", "祢", "祗", "祠", "祯", "祧", "祺", "禅",
        "禊", "禚", "禧", "禳", "忑", "忐", "怼", "恝", "恚", "恧", "恁", "恙", "恣", "悫", "愆", "愍",
        "慝", "憩", "憝", "懋", "懑", "戆", "肀", "聿", "沓", "泶", "淼", "矶", "矸", "砀", "砉", "砗",
        "砘", "砑", "斫", "砭", "砜", "砝", "砹", "砺", "砻", "砟", "砼", "砥", "砬", "砣", "砩", "硎",
        "硭", "硖", "硗", "砦", "硐", "硇", "硌", "硪", "碛", "碓", "碚", "碇", "碜", "碡", "碣", "碲",
        "碹", "碥", "磔", "磙", "磉", "磬", "磲", "礅", "磴", "礓", "礤", "礞", "礴", "龛", "黹", "黻",
        "黼", "盱", "眄", "眍", "盹", "眇", "眈", "眚", "眢", "眙", "眭", "眦", "眵", "眸", "睐", "睑",
        "睇", "睃", "睚", "睨", "睢", "睥", "睿", "瞍", "睽", "瞀", "瞌", "瞑", "瞟", "瞠", "瞰", "瞵",
        "瞽", "町", "畀", "畎", "畋", "畈", "畛", "畲", "畹", "疃", "罘", "罡", "罟", "詈", "罨", "罴",
        "罱", "罹", "羁", "罾", "盍", "盥", "蠲", "钅", "钆", "钇", "钋", "钊", "钌", "钍", "钏", "钐",
        "钔", "钗", "钕", "钚", "钛", "钜", "钣", "钤", "钫", "钪", "钭", "钬", "钯", "钰", "钲", "钴",
        "钶", "钷", "钸", "钹", "钺", "钼", "钽", "钿", "铄", "铈", "铉", "铊", "铋", "铌", "铍", "铎",
        "铐", "铑", "铒", "铕", "铖", "铗", "铙", "铘", "铛", "铞", "铟", "铠", "铢", "铤", "铥", "铧",
        "铨", "铪", "铩", "铫", "铮", "铯", "铳", "铴", "铵", "铷", "铹", "铼", "铽", "铿", "锃", "锂",
        "锆", "锇", "锉", "锊", "锍", "锎", "锏", "锒", "锓", "锔", "锕", "锖", "锘", "锛", "锝", "锞",
        "锟", "锢", "锪", "锫", "锩", "锬", "锱", "锲", "锴", "锶", "锷", "锸", "锼", "锾", "锿", "镂",
        "锵", "镄", "镅", "镆", "镉", "镌", "镎", "镏", "镒", "镓", "镔", "镖", "镗", "镘", "镙", "镛",
        "镞", "镟", "镝", "镡", "镢", "镤", "镥", "镦", "镧", "镨", "镩", "镪", "镫", "镬", "镯", "镱",
        "镲", "镳", "锺", "矧", "矬", "雉", "秕", "秭", "秣", "秫", "稆", "嵇", "稃", "稂", "稞", "稔",
        "稹", "稷", "穑", "黏", "馥", "穰", "皈", "皎", "皓", "皙", "皤", "瓞", "瓠", "甬", "鸠", "鸢",
        "鸨", "鸩", "鸪", "鸫", "鸬", "鸲", "鸱", "鸶", "鸸", "鸷", "鸹", "鸺", "鸾", "鹁", "鹂", "鹄",
        "鹆", "鹇", "鹈", "鹉", "鹋", "鹌", "鹎", "鹑", "鹕", "鹗", "鹚", "鹛", "鹜", "鹞", "鹣", "鹦",
        "鹧", "鹨", "鹩", "鹪", "鹫", "鹬", "鹱", "鹭", "鹳", "疒", "疔", "疖", "疠", "疝", "疬", "疣",
        "疳", "疴", "疸", "痄", "疱", "疰", "痃", "痂", "痖", "痍", "痣", "痨", "痦", "痤", "痫", "痧",
        "瘃", "痱", "痼", "痿", "瘐", "瘀", "瘅", "瘌", "瘗", "瘊", "瘥", "瘘", "瘕", "瘙", "瘛", "瘼",
        "瘢", "瘠", "癀", "瘭", "瘰", "瘿", "瘵", "癃", "瘾", "瘳", "癍", "癞", "癔", "癜", "癖", "癫",
        "癯", "翊", "竦", "穸", "穹", "窀", "窆", "窈", "窕", "窦", "窠", "窬", "窨", "窭", "窳", "衤",
        "衩", "衲", "衽", "衿", "袂", "袢", "裆", "袷", "袼", "裉", "裢", "裎", "裣", "裥", "裱", "褚",
        "裼", "裨", "裾", "裰", "褡", "褙", "褓", "褛", "褊", "褴", "褫", "褶", "襁", "襦", "襻", "疋",
        "胥", "皲", "皴", "矜", "耒", "耔", "耖", "耜", "耠", "耢", "耥", "耦", "耧", "耩", "耨", "耱",
        "耋", "耵", "聃", "聆", "聍", "聒", "聩", "聱", "覃", "顸", "颀", "颃", "颉", "颌", "颍", "颏",
        "颔", "颚", "颛", "颞", "颟", "颡", "颢", "颥", "颦", "虍", "虔", "虬", "虮", "虿", "虺", "虼",
        "虻", "蚨", "蚍", "蚋", "蚬", "蚝", "蚧", "蚣", "蚪", "蚓", "蚩", "蚶", "蛄", "蚵", "蛎", "蚰",
        "蚺", "蚱", "蚯", "蛉", "蛏", "蚴", "蛩", "蛱", "蛲", "蛭", "蛳", "蛐", "蜓", "蛞", "蛴", "蛟",
        "蛘", "蛑", "蜃", "蜇", "蛸", "蜈", "蜊", "蜍", "蜉", "蜣", "蜻", "蜞", "蜥", "蜮", "蜚", "蜾",
        "蝈", "蜴", "蜱", "蜩", "蜷", "蜿", "螂", "蜢", "蝽", "蝾", "蝻", "蝠", "蝰", "蝌", "蝮", "螋",
        "蝓", "蝣", "蝼", "蝤", "蝙", "蝥", "螓", "螯", "螨", "蟒", "蟆", "螈", "螅", "螭", "螗", "螃",
        "螫", "蟥", "螬", "螵", "螳", "蟋", "蟓", "螽", "蟑", "蟀", "蟊", "蟛", "蟪", "蟠", "蟮", "蠖",
        "蠓", "蟾", "蠊", "蠛", "蠡", "蠹", "蠼", "缶", "罂", "罄", "罅", "舐", "竺", "竽", "笈", "笃",
        "笄", "笕", "笊", "笫", "笏", "筇", "笸", "笪", "笙", "笮", "笱", "笠", "笥", "笤", "笳", "笾",
        "笞", "筘", "筚", "筅", "筵", "筌", "筝", "筠", "筮", "筻", "筢", "筲", "筱", "箐", "箦", "箧",
        "箸", "箬", "箝", "箨", "箅", "箪", "箜", "箢", "箫", "箴", "篑", "篁", "篌", "篝", "篚", "篥",
        "篦", "篪", "簌", "篾", "篼", "簏", "簖", "簋", "簟", "簪", "簦", "簸", "籁", "籀", "臾", "舁",
        "舂", "舄", "臬", "衄", "舡", "舢", "舣", "舭", "舯", "舨", "舫", "舸", "舻", "舳", "舴", "舾",
        "艄", "艉", "艋", "艏", "艚", "艟", "艨", "衾", "袅", "袈", "裘", "裟", "襞", "羝", "羟", "羧",
        "羯", "羰", "羲", "籼", "敉", "粑", "粝", "粜", "粞", "粢", "粲", "粼", "粽", "糁", "糇", "糌",
        "糍", "糈", "糅", "糗", "糨", "艮", "暨", "羿", "翎", "翕", "翥", "翡", "翦", "翩", "翮", "翳",
        "糸", "絷", "綦", "綮", "繇", "纛", "麸", "麴", "赳", "趄", "趔", "趑", "趱", "赧", "赭", "豇",
        "豉", "酊", "酐", "酎", "酏", "酤", "酢", "酡", "酰", "酩", "酯", "酽", "酾", "酲", "酴", "酹",
        "醌", "醅", "醐", "醍", "醑", "醢", "醣", "醪", "醭", "醮", "醯", "醵", "醴", "醺", "豕", "鹾",
        "趸", "跫", "踅", "蹙", "蹩", "趵", "趿", "趼", "趺", "跄", "跖", "跗", "跚", "跞", "跎", "跏",
        "跛", "跆", "跬", "跷", "跸", "跣", "跹", "跻", "跤", "踉", "跽", "踔", "踝", "踟", "踬", "踮",
        "踣", "踯", "踺", "蹀", "踹", "踵", "踽", "踱", "蹉", "蹁", "蹂", "蹑", "蹒", "蹊", "蹰", "蹶",
        "蹼", "蹯", "蹴", "躅", "躏", "躔", "躐", "躜", "躞", "豸", "貂", "貊", "貅", "貘", "貔", "斛",
        "觖", "觞", "觚", "觜", "觥", "觫", "觯", "訾", "謦", "靓", "雩", "雳", "雯", "霆", "霁", "霈",
        "霏", "霎", "霪", "霭", "霰", "霾", "龀", "龃", "龅", "龆", "龇", "龈", "龉", "龊", "龌", "黾",
        "鼋", "鼍", "隹", "隼", "隽", "雎", "雒", "瞿", "雠", "銎", "銮", "鋈", "錾", "鍪", "鏊", "鎏",
        "鐾", "鑫", "鱿", "鲂", "鲅", "鲆", "鲇", "鲈", "稣", "鲋", "鲎", "鲐", "鲑", "鲒", "鲔", "鲕",
        "鲚", "鲛", "鲞", "鲟", "鲠", "鲡", "鲢", "鲣", "鲥", "鲦", "鲧", "鲨", "鲩", "鲫", "鲭", "鲮",
        "鲰", "鲱", "鲲", "鲳", "鲴", "鲵", "鲶", "鲷", "鲺", "鲻", "鲼", "鲽", "鳄", "鳅", "鳆", "鳇",
        "鳊", "鳋", "鳌", "鳍", "鳎", "鳏", "鳐", "鳓", "鳔", "鳕", "鳗", "鳘", "鳙", "鳜", "鳝", "鳟",
        "鳢", "靼", "鞅", "鞑", "鞒", "鞔", "鞯", "鞫", "鞣", "鞲", "鞴", "骱", "骰", "骷", "鹘", "骶",
        "骺", "骼", "髁", "髀", "髅", "髂", "髋", "髌", "髑", "魅", "魃", "魇", "魉", "魈", "魍", "魑",
        "飨", "餍", "餮", "饕", "饔", "髟", "髡", "髦", "髯", "髫", "髻", "髭", "髹", "鬈", "鬏", "鬓",
        "鬟", "鬣", "麽", "麾", "縻", "麂", "麇", "麈", "麋", "麒", "鏖", "麝", "麟", "黛", "黜", "黝",
        "黠", "黟", "黢", "黩", "黧", "黥", "黪", "黯", "鼢", "鼬", "鼯", "鼹", "鼷", "鼽", "鼾", "齄"
     };
        #endregion
        #region otherPinYin
        private static string[] otherPinYin = new string[] {
        "chu", "ji", "wu", "gai", "nian", "sa", "pi", "gen", "cheng", "ge", "nao", "e", "shu", "yu", "pie", "bi",
        "tuo", "yao", "yao", "zhi", "di", "xin", "yin", "kui", "yu", "gao", "tao", "dian", "ji", "nai", "nie", "ji",
        "qi", "mi", "bei", "se", "gu", "ze", "she", "cuo", "yan", "jue", "si", "ye", "yan", "fang", "po", "gui",
        "kui", "bian", "ze", "gua", "you", "ce", "yi", "wen", "jing", "ku", "gui", "kai", "la", "ji", "yan", "wan",
        "kuai", "piao", "jue", "qiao", "huo", "yi", "tong", "wang", "dan", "ding", "zhang", "le", "sa", "yi", "mu", "ren",
        "yu", "pi", "ya", "wa", "wu", "chang", "cang", "kang", "zhu", "ning", "ka", "you", "yi", "gou", "tong", "tuo",
        "ni", "ga", "ji", "er", "you", "kua", "kan", "zhu", "yi", "tiao", "chai", "jiao", "nong", "mou", "chou", "yan",
        "li", "qiu", "li", "yu", "ping", "yong", "si", "feng", "qian", "ruo", "pai", "zhuo", "shu", "luo", "wo", "bi",
        "ti", "guan", "kong", "ju", "fen", "yan", "xie", "ji", "wei", "zong", "lou", "tang", "bin", "nuo", "chi", "xi",
        "jing", "jian", "jiao", "jiu", "tong", "xuan", "dan", "tong", "tun", "she", "qian", "zu", "yue", "cuan", "di", "xi",
        "xun", "hong", "guo", "chan", "kui", "bao", "pu", "hong", "fu", "fu", "su", "si", "wen", "yan", "bo", "gun",
        "mao", "xie", "luan", "pou", "bing", "ying", "luo", "lei", "liang", "hu", "lie", "xian", "song", "ping", "zhong", "ming",
        "yan", "jie", "hong", "shan", "ou", "ju", "ne", "gu", "he", "di", "zhao", "qu", "dai", "kuang", "lei", "gua",
        "jie", "hui", "shen", "gou", "quan", "zheng", "hun", "xu", "qiao", "gao", "kuang", "ei", "zou", "zhuo", "wei", "yu",
        "shen", "chan", "sui", "chen", "jian", "xue", "ye", "e", "yu", "xuan", "an", "di", "zi", "pian", "mo", "dang",
        "su", "shi", "mi", "zhe", "jian", "zen", "qiao", "jue", "yan", "zhan", "chen", "dan", "jin", "zuo", "wu", "qian",
        "jing", "ban", "yan", "zuo", "bei", "jing", "gai", "zhi", "nie", "zou", "chui", "pi", "wei", "huang", "wei", "xi",
        "han", "qiong", "kuang", "mang", "wu", "fang", "bing", "pi", "bei", "ye", "di", "tai", "jia", "zhi", "zhu", "kuai",
        "qie", "xun", "yun", "li", "ying", "gao", "xi", "fu", "pi", "tan", "yan", "juan", "yan", "yin", "zhang", "po",
        "shan", "zou", "ling", "feng", "chu", "huan", "mai", "qu", "shao", "he", "ge", "meng", "xu", "xie", "sou", "xie",
        "jue", "jian", "qian", "dang", "chang", "si", "bian", "ben", "qiu", "ben", "e", "fa", "shu", "ji", "yong", "he",
        "wei", "wu", "ge", "zhen", "kuang", "pi", "yi", "li", "qi", "ban", "gan", "long", "dian", "lu", "che", "di",
        "tuo", "ni", "mu", "ao", "ya", "die", "dong", "kai", "shan", "shang", "nao", "gai", "yin", "cheng", "shi", "guo",
        "xun", "lie", "yuan", "zhi", "an", "yi", "pi", "nian", "peng", "tu", "sao", "dai", "ku", "die", "yin", "leng",
        "hou", "ge", "yuan", "man", "yong", "liang", "chi", "xin", "pi", "yi", "cao", "jiao", "nai", "du", "qian", "ji",
        "wan", "xiong", "qi", "xiang", "fu", "yuan", "yun", "fei", "ji", "li", "e", "ju", "pi", "zhi", "rui", "xian",
        "chang", "cong", "qin", "wu", "qian", "qi", "shan", "bian", "zhu", "kou", "yi", "mo", "gan", "pie", "long", "ba",
        "mu", "ju", "ran", "qing", "chi", "fu", "ling", "niao", "yin", "mao", "ying", "qiong", "min", "tiao", "qian", "yi",
        "rao", "bi", "zi", "ju", "tong", "hui", "zhu", "ting", "qiao", "fu", "ren", "xing", "quan", "hui", "xun", "ming",
        "qi", "jiao", "chong", "jiang", "luo", "ying", "qian", "gen", "jin", "mai", "sun", "hong", "zhou", "kan", "bi", "shi",
        "wo", "you", "e", "mei", "you", "li", "tu", "xian", "fu", "sui", "you", "di", "shen", "guan", "lang", "ying",
        "chun", "jing", "qi", "xi", "song", "jin", "nai", "qi", "ba", "shu", "chang", "tie", "yu", "huan", "bi", "fu",
        "tu", "dan", "cui", "yan", "zu", "dang", "jian", "wan", "ying", "gu", "han", "qia", "feng", "shen", "xiang", "wei",
        "chan", "kai", "qi", "kui", "xi", "e", "bao", "pa", "ting", "lou", "pai", "xuan", "jia", "zhen", "shi", "ru",
        "mo", "en", "bei", "weng", "hao", "ji", "li", "bang", "jian", "shuo", "lang", "ying", "yu", "su", "meng", "dou",
        "xi", "lian", "cu", "lin", "qu", "kou", "xu", "liao", "hui", "xun", "jue", "rui", "zui", "ji", "meng", "fan",
        "qi", "hong", "xie", "hong", "wei", "yi", "weng", "sou", "bi", "hao", "tai", "ru", "xun", "xian", "gao", "li",
        "huo", "qu", "heng", "fan", "nie", "mi", "gong", "yi", "kuang", "lian", "da", "yi", "xi", "zang", "pao", "you",
        "liao", "ga", "gan", "ti", "men", "tuan", "chen", "fu", "pin", "niu", "jie", "jiao", "za", "yi", "lv", "jun",
        "tian", "ye", "ai", "na", "ji", "guo", "bai", "ju", "pou", "lie", "qian", "guan", "die", "zha", "ya", "qin",
        "yu", "an", "xuan", "bing", "kui", "yuan", "shu", "en", "chuai", "jian", "shuo", "zhan", "nuo", "sang", "luo", "ying",
        "zhi", "han", "zhe", "xie", "lu", "zun", "cuan", "gan", "huan", "pi", "xing", "zhuo", "huo", "zuan", "nang", "yi",
        "te", "dai", "shi", "bu", "chi", "ji", "kou", "dao", "le", "zha", "a", "yao", "fu", "mu", "yi", "tai",
        "li", "e", "bi", "bei", "guo", "qin", "yin", "za", "ka", "ga", "gua", "ling", "dong", "ning", "duo", "nao",
        "you", "si", "kuang", "ji", "shen", "hui", "da", "lie", "yi", "xiao", "bi", "ci", "guang", "yue", "xiu", "yi",
        "pai", "kuai", "duo", "ji", "mie", "mi", "zha", "nong", "gen", "mou", "mai", "chi", "lao", "geng", "en", "zha",
        "suo", "zao", "xi", "zuo", "ji", "feng", "ze", "nuo", "miao", "lin", "zhuan", "zhou", "tao", "hu", "cui", "sha",
        "yo", "dan", "bo", "ding", "lang", "li", "shua", "chuo", "die", "da", "nan", "li", "kui", "jie", "yong", "kui",
        "jiu", "sou", "yin", "chi", "jie", "lou", "ku", "wo", "hui", "qin", "ao", "su", "du", "ke", "nie", "he",
        "chen", "suo", "ge", "a", "en", "hao", "dia", "ai", "ai", "suo", "hei", "tong", "chi", "pei", "lei", "cao",
        "piao", "qi", "ying", "beng", "sou", "di", "mi", "peng", "jue", "liao", "pu", "chuai", "jiao", "o", "qin", "lu",
        "ceng", "deng", "hao", "jin", "jue", "yi", "sai", "pi", "ru", "cha", "huo", "nang", "wei", "jian", "nan", "lun",
        "hu", "ling", "you", "yu", "qing", "yu", "huan", "wei", "zhi", "pei", "tang", "dao", "ze", "guo", "wei", "wo",
        "man", "zhang", "fu", "fan", "ji", "qi", "qian", "qi", "qu", "ya", "xian", "ao", "cen", "lan", "ba", "hu",
        "ke", "dong", "jia", "xiu", "dai", "gou", "mao", "min", "yi", "dong", "qiao", "xun", "zheng", "lao", "lai", "song",
        "yan", "gu", "xiao", "guo", "kong", "jue", "rong", "yao", "wai", "zai", "wei", "yu", "cuo", "lou", "zi", "mei",
        "sheng", "song", "ji", "zhang", "lin", "deng", "bin", "yi", "dian", "chi", "pang", "cu", "xun", "yang", "hou", "lai",
        "xi", "chang", "huang", "yao", "zheng", "jiao", "qu", "san", "fan", "qiu", "an", "guang", "ma", "niu", "yun", "xia",
        "pao", "fei", "rong", "kuai", "shou", "sun", "bi", "juan", "li", "yu", "xian", "yin", "suan", "yi", "guo", "luo",
        "ni", "she", "cu", "mi", "hu", "cha", "wei", "wei", "mei", "nao", "zhang", "jing", "jue", "liao", "xie", "xun",
        "huan", "chuan", "huo", "sun", "yin", "dong", "shi", "tang", "tun", "xi", "ren", "yu", "chi", "yi", "xiang", "bo",
        "yu", "hun", "zha", "sou", "mo", "xiu", "jin", "san", "zhuan", "nang", "pi", "wu", "gui", "pao", "xiu", "xiang",
        "tuo", "an", "yu", "bi", "geng", "ao", "jin", "chan", "xie", "lin", "ying", "shu", "dao", "cun", "chan", "wu",
        "zhi", "ou", "chong", "wu", "kai", "chang", "chuang", "song", "bian", "niu", "hu", "chu", "peng", "da", "yang", "zuo",
        "ni", "fu", "chao", "yi", "yi", "tong", "yan", "ce", "kai", "xun", "ke", "yun", "bei", "song", "qian", "kui",
        "kun", "yi", "ti", "quan", "qie", "xing", "fei", "chang", "wang", "chou", "hu", "cui", "yun", "kui", "e", "leng",
        "zhui", "qiao", "bi", "su", "qie", "yong", "jing", "qiao", "chong", "chu", "lin", "meng", "tian", "hui", "shuan", "yan",
        "wei", "hong", "min", "kang", "ta", "lv", "kun", "jiu", "lang", "yu", "chang", "xi", "wen", "hun", "e", "qu",
        "que", "he", "tian", "que", "kan", "jiang", "pan", "qiang", "san", "qi", "si", "cha", "feng", "yuan", "mu", "mian",
        "dun", "mi", "gu", "bian", "wen", "hang", "wei", "le", "gan", "shu", "long", "lu", "yang", "si", "duo", "ling",
        "mao", "luo", "xuan", "pan", "duo", "hong", "min", "jing", "huan", "wei", "lie", "jia", "zhen", "yin", "hui", "zhu",
        "ji", "xu", "hui", "tao", "xun", "jiang", "liu", "hu", "xun", "ru", "su", "wu", "lai", "wei", "zhuo", "juan",
        "cen", "bang", "xi", "mei", "huan", "zhu", "qi", "xi", "song", "du", "zhuo", "pei", "mian", "gan", "fei", "cong",
        "shen", "guan", "lu", "shuan", "xie", "yan", "mian", "qiu", "sou", "huang", "xu", "pen", "jian", "xuan", "wo", "mei",
        "yan", "qin", "ke", "she", "mang", "ying", "pu", "li", "ru", "ta", "hun", "bi", "xiu", "fu", "tang", "pang",
        "ming", "huang", "ying", "xiao", "lan", "cao", "hu", "luo", "huan", "lian", "zhu", "yi", "lu", "xuan", "gan", "shu",
        "si", "shan", "shao", "tong", "chan", "lai", "sui", "li", "dan", "chan", "lian", "ru", "pu", "bi", "hao", "zhuo",
        "han", "xie", "ying", "yue", "fen", "hao", "ba", "bao", "gui", "dang", "mi", "you", "chen", "ning", "jian", "qian",
        "wu", "liao", "qian", "huan", "jian", "jian", "zou", "ya", "wu", "jiong", "ze", "yi", "er", "jia", "jing", "dai",
        "hou", "pang", "bu", "li", "qiu", "xiao", "ti", "qun", "kui", "wei", "huan", "lu", "chuan", "huang", "qiu", "xia",
        "ao", "gou", "ta", "liu", "xian", "lin", "ju", "xie", "miao", "sui", "la", "ji", "hui", "tuan", "zhi", "kao",
        "zhi", "ji", "e", "chan", "xi", "ju", "chan", "jing", "nu", "mi", "fu", "bi", "yu", "che", "shuo", "fei",
        "yan", "wu", "yu", "bi", "jin", "zi", "gui", "niu", "yu", "si", "da", "zhou", "shan", "qie", "ya", "rao",
        "shu", "luan", "jiao", "pin", "cha", "li", "ping", "wa", "xian", "suo", "di", "wei", "e", "jing", "biao", "jie",
        "chang", "bi", "chan", "nu", "ao", "yuan", "ting", "wu", "gou", "mo", "pi", "ai", "pin", "chi", "li", "yan",
        "qiang", "piao", "chang", "lei", "zhang", "xi", "shan", "bi", "niao", "mo", "shuang", "ga", "ga", "fu", "nu", "zi",
        "jie", "jue", "bao", "zang", "si", "fu", "zou", "yi", "nu", "dai", "xiao", "hua", "pian", "li", "qi", "ke",
        "zhui", "can", "zhi", "wu", "ao", "liu", "shan", "biao", "cong", "chan", "ji", "xiang", "jiao", "yu", "zhou", "ge",
        "wan", "kuang", "yun", "pi", "shu", "gan", "xie", "fu", "zhou", "fu", "chu", "dai", "ku", "hang", "jiang", "geng",
        "xiao", "ti", "ling", "qi", "fei", "shang", "gun", "duo", "shou", "liu", "quan", "wan", "zi", "ke", "xiang", "ti",
        "miao", "hui", "si", "bian", "gou", "zhui", "min", "jin", "zhen", "ru", "gao", "li", "yi", "jian", "bin", "piao",
        "man", "lei", "miao", "sao", "xie", "liao", "zeng", "jiang", "qian", "qiao", "huan", "zuan", "yao", "ji", "chuan", "zai",
        "yong", "ding", "ji", "wei", "bin", "min", "jue", "ke", "long", "dian", "dai", "po", "min", "jia", "er", "gong",
        "xu", "ya", "heng", "yao", "luo", "xi", "hui", "lian", "qi", "ying", "qi", "hu", "kun", "yan", "cong", "wan",
        "chen", "ju", "mao", "yu", "yuan", "xia", "nao", "ai", "tang", "jin", "huang", "ying", "cui", "cong", "xuan", "zhang",
        "pu", "can", "qu", "lu", "bi", "zan", "wen", "wei", "yun", "tao", "wu", "shao", "qi", "cha", "ma", "li",
        "pi", "miao", "yao", "rui", "jian", "chu", "cheng", "cong", "xiao", "fang", "pa", "zhu", "nai", "zhi", "zhe", "long",
        "jiu", "ping", "lu", "xia", "xiao", "you", "zhi", "tuo", "zhi", "ling", "gou", "di", "li", "tuo", "cheng", "kao",
        "lao", "ya", "rao", "zhi", "zhen", "guang", "qi", "ting", "gua", "jiu", "hua", "heng", "gui", "jie", "luan", "juan",
        "an", "xu", "fan", "gu", "fu", "jue", "zi", "suo", "ling", "chu", "fen", "du", "qian", "zhao", "luo", "chui",
        "liang", "guo", "jian", "di", "ju", "cou", "zhen", "nan", "zha", "lian", "lan", "ji", "pin", "ju", "qiu", "duan",
        "chui", "chen", "lv", "cha", "ju", "xuan", "mei", "ying", "zhen", "fei", "ta", "sun", "xie", "gao", "cui", "gao",
        "shuo", "bin", "rong", "zhu", "xie", "jin", "qiang", "qi", "chu", "tang", "zhu", "hu", "gan", "yue", "qing", "tuo",
        "jue", "qiao", "qin", "lu", "zun", "xi", "ju", "yuan", "lei", "yan", "lin", "bo", "cha", "you", "ao", "mo",
        "cu", "shang", "tian", "yun", "lian", "piao", "dan", "ji", "bin", "yi", "ren", "e", "gu", "ke", "lu", "zhi",
        "yi", "zhen", "hu", "li", "yao", "shi", "zhi", "quan", "lu", "zhe", "nian", "wang", "chuo", "zi", "cou", "lu",
        "lin", "wei", "jian", "qiang", "jia", "ji", "ji", "kan", "deng", "gai", "jian", "zang", "ou", "ling", "bu", "beng",
        "zeng", "pi", "po", "ga", "la", "gan", "hao", "tan", "gao", "ze", "xin", "yun", "gui", "he", "zan", "mao",
        "yu", "chang", "ni", "qi", "sheng", "ye", "chao", "yan", "hui", "bu", "han", "gui", "xuan", "kui", "ai", "ming",
        "tun", "xun", "yao", "xi", "nang", "ben", "shi", "kuang", "yi", "zhi", "zi", "gai", "jin", "zhen", "lai", "qiu",
        "ji", "dan", "fu", "chan", "ji", "xi", "di", "yu", "gou", "jin", "qu", "jian", "jiang", "pin", "mao", "gu",
        "wu", "gu", "ji", "ju", "jian", "pian", "kao", "qie", "suo", "bai", "ge", "bo", "mao", "mu", "cui", "jian",
        "san", "shu", "chang", "lu", "pu", "qu", "pie", "dao", "xian", "chuan", "dong", "ya", "yin", "ke", "yun", "fan",
        "chi", "jiao", "du", "die", "you", "yuan", "guo", "yue", "wo", "rong", "huang", "jing", "ruan", "tai", "gong", "zhun",
        "na", "yao", "qian", "long", "dong", "ka", "lu", "jia", "shen", "zhou", "zuo", "gua", "zhen", "qu", "zhi", "jing",
        "guang", "dong", "yan", "kuai", "sa", "hai", "pian", "zhen", "mi", "tun", "luo", "cuo", "pao", "wan", "niao", "jing",
        "yan", "fei", "yu", "zong", "ding", "jian", "cou", "nan", "mian", "wa", "e", "shu", "cheng", "ying", "ge", "lv",
        "bin", "teng", "zhi", "chuai", "gu", "meng", "sao", "shan", "lian", "lin", "yu", "xi", "qi", "sha", "xin", "xi",
        "biao", "sa", "ju", "sou", "biao", "biao", "shu", "gou", "gu", "hu", "fei", "ji", "lan", "yu", "pei", "mao",
        "zhan", "jing", "ni", "liu", "yi", "yang", "wei", "dun", "qiang", "shi", "hu", "zhu", "xuan", "tai", "ye", "yang",
        "wu", "han", "men", "chao", "yan", "hu", "yu", "wei", "duan", "bao", "xuan", "bian", "tui", "liu", "man", "shang",
        "yun", "yi", "yu", "fan", "sui", "xian", "jue", "cuan", "huo", "tao", "xu", "xi", "li", "hu", "jiong", "hu",
        "fei", "shi", "si", "xian", "zhi", "qu", "hu", "fu", "zuo", "mi", "zhi", "ci", "zhen", "tiao", "qi", "chan",
        "xi", "zhuo", "xi", "rang", "te", "tan", "dui", "jia", "hui", "nv", "nin", "yang", "zi", "que", "qian", "min",
        "te", "qi", "dui", "mao", "men", "gang", "yu", "yu", "ta", "xue", "miao", "ji", "gan", "dang", "hua", "che",
        "dun", "ya", "zhuo", "bian", "feng", "fa", "ai", "li", "long", "zha", "tong", "di", "la", "tuo", "fu", "xing",
        "mang", "xia", "qiao", "zhai", "dong", "nao", "ge", "wo", "qi", "dui", "bei", "ding", "chen", "zhou", "jie", "di",
        "xuan", "bian", "zhe", "gun", "sang", "qing", "qu", "dun", "deng", "jiang", "ca", "meng", "bo", "kan", "zhi", "fu",
        "fu", "xu", "mian", "kou", "dun", "miao", "dan", "sheng", "yuan", "yi", "sui", "zi", "chi", "mou", "lai", "jian",
        "di", "suo", "ya", "ni", "sui", "pi", "rui", "sou", "kui", "mao", "ke", "ming", "piao", "cheng", "kan", "lin",
        "gu", "ding", "bi", "quan", "tian", "fan", "zhen", "she", "wan", "tuan", "fu", "gang", "gu", "li", "yan", "pi",
        "lan", "li", "ji", "zeng", "he", "guan", "juan", "jin", "ga", "yi", "po", "zhao", "liao", "tu", "chuan", "shan",
        "men", "chai", "nv", "bu", "tai", "ju", "ban", "qian", "fang", "kang", "dou", "huo", "ba", "yu", "zheng", "gu",
        "ke", "po", "bu", "bo", "yue", "mu", "tan", "dian", "shuo", "shi", "xuan", "ta", "bi", "ni", "pi", "duo",
        "kao", "lao", "er", "you", "cheng", "jia", "nao", "ye", "cheng", "diao", "yin", "kai", "zhu", "ding", "diu", "hua",
        "quan", "ha", "sha", "diao", "zheng", "se", "chong", "tang", "an", "ru", "lao", "lai", "te", "keng", "zeng", "li",
        "gao", "e", "cuo", "lve", "liu", "kai", "jian", "lang", "qin", "ju", "a", "qiang", "nuo", "ben", "de", "ke",
        "kun", "gu", "huo", "pei", "juan", "tan", "zi", "qie", "kai", "si", "e", "cha", "sou", "huan", "ai", "lou",
        "qiang", "fei", "mei", "mo", "ge", "juan", "na", "liu", "yi", "jia", "bin", "biao", "tang", "man", "luo", "yong",
        "chuo", "xuan", "di", "tan", "jue", "pu", "lu", "dui", "lan", "pu", "cuan", "qiang", "deng", "huo", "zhuo", "yi",
        "cha", "biao", "zhong", "shen", "cuo", "zhi", "bi", "zi", "mo", "shu", "lv", "ji", "fu", "lang", "ke", "ren",
        "zhen", "ji", "se", "nian", "fu", "rang", "gui", "jiao", "hao", "xi", "po", "die", "hu", "yong", "jiu", "yuan",
        "bao", "zhen", "gu", "dong", "lu", "qu", "chi", "si", "er", "zhi", "gua", "xiu", "luan", "bo", "li", "hu",
        "yu", "xian", "ti", "wu", "miao", "an", "bei", "chun", "hu", "e", "ci", "mei", "wu", "yao", "jian", "ying",
        "zhe", "liu", "liao", "jiao", "jiu", "yu", "hu", "lu", "guan", "bing", "ding", "jie", "li", "shan", "li", "you",
        "gan", "ke", "da", "zha", "pao", "zhu", "xuan", "jia", "ya", "yi", "zhi", "lao", "wu", "cuo", "xian", "sha",
        "zhu", "fei", "gu", "wei", "yu", "yu", "dan", "la", "yi", "hou", "chai", "lou", "jia", "sao", "chi", "mo",
        "ban", "ji", "huang", "biao", "luo", "ying", "zhai", "long", "yin", "chou", "ban", "lai", "yi", "dian", "pi", "dian",
        "qu", "yi", "song", "xi", "qiong", "zhun", "bian", "yao", "tiao", "dou", "ke", "yu", "xun", "ju", "yu", "yi",
        "cha", "na", "ren", "jin", "mei", "pan", "dang", "jia", "ge", "ken", "lian", "cheng", "lian", "jian", "biao", "chu",
        "ti", "bi", "ju", "duo", "da", "bei", "bao", "lv", "bian", "lan", "chi", "zhe", "qiang", "ru", "pan", "ya",
        "xu", "jun", "cun", "jin", "lei", "zi", "chao", "si", "huo", "lao", "tang", "ou", "lou", "jiang", "nou", "mo",
        "die", "ding", "dan", "ling", "ning", "guo", "kui", "ao", "qin", "han", "qi", "hang", "jie", "he", "ying", "ke",
        "han", "e", "zhuan", "nie", "man", "sang", "hao", "ru", "pin", "hu", "qian", "qiu", "ji", "chai", "hui", "ge",
        "meng", "fu", "pi", "rui", "xian", "hao", "jie", "gong", "dou", "yin", "chi", "han", "gu", "ke", "li", "you",
        "ran", "zha", "qiu", "ling", "cheng", "you", "qiong", "jia", "nao", "zhi", "si", "qu", "ting", "kuo", "qi", "jiao",
        "yang", "mou", "shen", "zhe", "shao", "wu", "li", "chu", "fu", "qiang", "qing", "qi", "xi", "yu", "fei", "guo",
        "guo", "yi", "pi", "tiao", "quan", "wan", "lang", "meng", "chun", "rong", "nan", "fu", "kui", "ke", "fu", "sou",
        "yu", "you", "lou", "you", "bian", "mou", "qin", "ao", "man", "mang", "ma", "yuan", "xi", "chi", "tang", "pang",
        "shi", "huang", "cao", "piao", "tang", "xi", "xiang", "zhong", "zhang", "shuai", "mao", "peng", "hui", "pan", "shan", "huo",
        "meng", "chan", "lian", "mie", "li", "du", "qu", "fou", "ying", "qing", "xia", "shi", "zhu", "yu", "ji", "du",
        "ji", "jian", "zhao", "zi", "hu", "qiong", "po", "da", "sheng", "ze", "gou", "li", "si", "tiao", "jia", "bian",
        "chi", "kou", "bi", "xian", "yan", "quan", "zheng", "jun", "shi", "gang", "pa", "shao", "xiao", "qing", "ze", "qie",
        "zhu", "ruo", "qian", "tuo", "bi", "dan", "kong", "wan", "xiao", "zhen", "kui", "huang", "hou", "gou", "fei", "li",
        "bi", "chi", "su", "mie", "dou", "lu", "duan", "gui", "dian", "zan", "deng", "bo", "lai", "zhou", "yu", "yu",
        "chong", "xi", "nie", "nv", "chuan", "shan", "yi", "bi", "zhong", "ban", "fang", "ge", "lu", "zhu", "ze", "xi",
        "shao", "wei", "meng", "shou", "cao", "chong", "meng", "qin", "niao", "jia", "qiu", "sha", "bi", "di", "qiang", "suo",
        "jie", "tang", "xi", "xian", "mi", "ba", "li", "tiao", "xi", "zi", "can", "lin", "zong", "san", "hou", "zan",
        "ci", "xu", "rou", "qiu", "jiang", "gen", "ji", "yi", "ling", "xi", "zhu", "fei", "jian", "pian", "he", "yi",
        "jiao", "zhi", "qi", "qi", "yao", "dao", "fu", "qu", "jiu", "ju", "lie", "zi", "zan", "nan", "zhe", "jiang",
        "chi", "ding", "gan", "zhou", "yi", "gu", "zuo", "tuo", "xian", "ming", "zhi", "yan", "shai", "cheng", "tu", "lei",
        "kun", "pei", "hu", "ti", "xu", "hai", "tang", "lao", "bu", "jiao", "xi", "ju", "li", "xun", "shi", "cuo",
        "dun", "qiong", "xue", "cu", "bie", "bo", "ta", "jian", "fu", "qiang", "zhi", "fu", "shan", "li", "tuo", "jia",
        "bo", "tai", "kui", "qiao", "bi", "xian", "xian", "ji", "jiao", "liang", "ji", "chuo", "huai", "chi", "zhi", "dian",
        "bo", "zhi", "jian", "die", "chuai", "zhong", "ju", "duo", "cuo", "pian", "rou", "nie", "pan", "qi", "chu", "jue",
        "pu", "fan", "cu", "zhu", "lin", "chan", "lie", "zuan", "xie", "zhi", "diao", "mo", "xiu", "mo", "pi", "hu",
        "jue", "shang", "gu", "zi", "gong", "su", "zhi", "zi", "qing", "liang", "yu", "li", "wen", "ting", "ji", "pei",
        "fei", "sha", "yin", "ai", "xian", "mai", "chen", "ju", "bao", "tiao", "zi", "yin", "yu", "chuo", "wo", "mian",
        "yuan", "tuo", "zhui", "sun", "jun", "ju", "luo", "qu", "chou", "qiong", "luan", "wu", "zan", "mou", "ao", "liu",
        "bei", "xin", "you", "fang", "ba", "ping", "nian", "lu", "su", "fu", "hou", "tai", "gui", "jie", "wei", "er",
        "ji", "jiao", "xiang", "xun", "geng", "li", "lian", "jian", "shi", "tiao", "gun", "sha", "huan", "ji", "qing", "ling",
        "zou", "fei", "kun", "chang", "gu", "ni", "nian", "diao", "shi", "zi", "fen", "die", "e", "qiu", "fu", "huang",
        "bian", "sao", "ao", "qi", "ta", "guan", "yao", "le", "biao", "xue", "man", "min", "yong", "gui", "shan", "zun",
        "li", "da", "yang", "da", "qiao", "man", "jian", "ju", "rou", "gou", "bei", "jie", "tou", "ku", "gu", "di",
        "hou", "ge", "ke", "bi", "lou", "qia", "kuan", "bin", "du", "mei", "ba", "yan", "liang", "xiao", "wang", "chi",
        "xiang", "yan", "tie", "tao", "yong", "biao", "kun", "mao", "ran", "tiao", "ji", "zi", "xiu", "quan", "jiu", "bin",
        "huan", "lie", "me", "hui", "mi", "ji", "jun", "zhu", "mi", "qi", "ao", "she", "lin", "dai", "chu", "you",
        "xia", "yi", "qu", "du", "li", "qing", "can", "an", "fen", "you", "wu", "yan", "xi", "qiu", "han", "zha"
        };
        #endregion
        #region pinYinArray
        private static string[] pinYinArray = new string[] {
            "a", "b", "c", "d", "e", "f", "g", "h", "j", "k", "l", "m", "n", "o", "p", "q",
            "r", "s", "t", "w", "x", "y", "z"
         };
        #endregion
        #region pyName
        private static string[] pyName = new string[] {
        "a", "ai", "an", "ang", "ao", "ba", "bai", "ban", "bang", "bao", "bei", "ben", "beng", "bi", "bian", "biao",
        "bie", "bin", "bing", "bo", "bu", "ba", "cai", "can", "cang", "cao", "ce", "ceng", "cha", "chai", "chan", "chang",
        "chao", "che", "chen", "cheng", "chi", "chong", "chou", "chu", "chuai", "chuan", "chuang", "chui", "chun", "chuo", "ci", "cong",
        "cou", "cu", "cuan", "cui", "cun", "cuo", "da", "dai", "dan", "dang", "dao", "de", "deng", "di", "dian", "diao",
        "die", "ding", "diu", "dong", "dou", "du", "duan", "dui", "dun", "duo", "e", "en", "er", "fa", "fan", "fang",
        "fei", "fen", "feng", "fo", "fou", "fu", "ga", "gai", "gan", "gang", "gao", "ge", "gei", "gen", "geng", "gong",
        "gou", "gu", "gua", "guai", "guan", "guang", "gui", "gun", "guo", "ha", "hai", "han", "hang", "hao", "he", "hei",
        "hen", "heng", "hong", "hou", "hu", "hua", "huai", "huan", "huang", "hui", "hun", "huo", "ji", "jia", "jian", "jiang",
        "jiao", "jie", "jin", "jing", "jiong", "jiu", "ju", "juan", "jue", "jun", "ka", "kai", "kan", "kang", "kao", "ke",
        "ken", "keng", "kong", "kou", "ku", "kua", "kuai", "kuan", "kuang", "kui", "kun", "kuo", "la", "lai", "lan", "lang",
        "lao", "le", "lei", "leng", "li", "lia", "lian", "liang", "liao", "lie", "lin", "ling", "liu", "long", "lou", "lu",
        "lv", "luan", "lue", "lun", "luo", "ma", "mai", "man", "mang", "mao", "me", "mei", "men", "meng", "mi", "mian",
        "miao", "mie", "min", "ming", "miu", "mo", "mou", "mu", "na", "nai", "nan", "nang", "nao", "ne", "nei", "nen",
        "neng", "ni", "nian", "niang", "niao", "nie", "nin", "ning", "niu", "nong", "nu", "nv", "nuan", "nue", "nuo", "o",
        "ou", "pa", "pai", "pan", "pang", "pao", "pei", "pen", "peng", "pi", "pian", "piao", "pie", "pin", "ping", "po",
        "pu", "qi", "qia", "qian", "qiang", "qiao", "qie", "qin", "qing", "qiong", "qiu", "qu", "quan", "que", "qun", "ran",
        "rang", "rao", "re", "ren", "reng", "ri", "rong", "rou", "ru", "ruan", "rui", "run", "ruo", "sa", "sai", "san",
        "sang", "sao", "se", "sen", "seng", "sha", "shai", "shan", "shang", "shao", "she", "shen", "sheng", "shi", "shou", "shu",
        "shua", "shuai", "shuan", "shuang", "shui", "shun", "shuo", "si", "song", "sou", "su", "suan", "sui", "sun", "suo", "ta",
        "tai", "tan", "tang", "tao", "te", "teng", "ti", "tian", "tiao", "tie", "ting", "tong", "tou", "tu", "tuan", "tui",
        "tun", "tuo", "wa", "wai", "wan", "wang", "wei", "wen", "weng", "wo", "wu", "xi", "xia", "xian", "xiang", "xiao",
        "xie", "xin", "xing", "xiong", "xiu", "xu", "xuan", "xue", "xun", "ya", "yan", "yang", "yao", "ye", "yi", "yin",
        "ying", "yo", "yong", "you", "yu", "yuan", "yue", "yun", "za", "zai", "zan", "zang", "zao", "ze", "zei", "zen",
        "zeng", "zha", "zhai", "zhan", "zhang", "zhao", "zhe", "zhen", "zheng", "zhi", "zhong", "zhou", "zhu", "zhua", "zhuai", "zhuan",
        "zhuang", "zhui", "zhun", "zhuo", "zi", "zong", "zou", "zu", "zuan", "zui", "zun", "zuo"
     };
        #endregion
        #region pyValue
        private static int[] pyValue = new int[] {
        -20319, -20317, -20304, -20295, -20292, -20283, -20265, -20257, -20242, -20230, -20051, -20036, -20032, -20026, -20002, -19990,
        -19986, -19982, -19976, -19805, -19784, -19775, -19774, -19763, -19756, -19751, -19746, -19741, -19739, -19728, -19725, -19715,
        -19540, -19531, -19525, -19515, -19500, -19484, -19479, -19467, -19289, -19288, -19281, -19275, -19270, -19263, -19261, -19249,
        -19243, -19242, -19238, -19235, -19227, -19224, -19218, -19212, -19038, -19023, -19018, -19006, -19003, -18996, -18977, -18961,
        -18952, -18783, -18774, -18773, -18763, -18756, -18741, -18735, -18731, -18722, -18710, -18697, -18696, -18526, -18518, -18501,
        -18490, -18478, -18463, -18448, -18447, -18446, -18239, -18237, -18231, -18220, -18211, -18201, -18184, -18183, -18181, -18012,
        -17997, -17988, -17970, -17964, -17961, -17950, -17947, -17931, -17928, -17922, -17759, -17752, -17733, -17730, -17721, -17703,
        -17701, -17697, -17692, -17683, -17676, -17496, -17487, -17482, -17468, -17454, -17433, -17427, -17417, -17202, -17185, -16983,
        -16970, -16942, -16915, -16733, -16708, -16706, -16689, -16664, -16657, -16647, -16474, -16470, -16465, -16459, -16452, -16448,
        -16433, -16429, -16427, -16423, -16419, -16412, -16407, -16403, -16401, -16393, -16220, -16216, -16212, -16205, -16202, -16187,
        -16180, -16171, -16169, -16158, -16155, -15959, -15958, -15944, -15933, -15920, -15915, -15903, -15889, -15878, -15707, -15701,
        -15681, -15667, -15661, -15659, -15652, -15640, -15631, -15625, -15454, -15448, -15436, -15435, -15419, -15416, -15408, -15394,
        -15385, -15377, -15375, -15369, -15363, -15362, -15183, -15180, -15165, -15158, -15153, -15150, -15149, -15144, -15143, -15141,
        -15140, -15139, -15128, -15121, -15119, -15117, -15110, -15109, -14941, -14937, -14933, -14930, -14929, -14928, -14926, -14922,
        -14921, -14914, -14908, -14902, -14894, -14889, -14882, -14873, -14871, -14857, -14678, -14674, -14670, -14668, -14663, -14654,
        -14645, -14630, -14594, -14429, -14407, -14399, -14384, -14379, -14368, -14355, -14353, -14345, -14170, -14159, -14151, -14149,
        -14145, -14140, -14137, -14135, -14125, -14123, -14122, -14112, -14109, -14099, -14097, -14094, -14092, -14090, -14087, -14083,
        -13917, -13914, -13910, -13907, -13906, -13905, -13896, -13894, -13878, -13870, -13859, -13847, -13831, -13658, -13611, -13601,
        -13406, -13404, -13400, -13398, -13395, -13391, -13387, -13383, -13367, -13359, -13356, -13343, -13340, -13329, -13326, -13318,
        -13147, -13138, -13120, -13107, -13096, -13095, -13091, -13076, -13068, -13063, -13060, -12888, -12875, -12871, -12860, -12858,
        -12852, -12849, -12838, -12831, -12829, -12812, -12802, -12607, -12597, -12594, -12585, -12556, -12359, -12346, -12320, -12300,
        -12120, -12099, -12089, -12074, -12067, -12058, -12039, -11867, -11861, -11847, -11831, -11798, -11781, -11604, -11589, -11536,
        -11358, -11340, -11339, -11324, -11303, -11097, -11077, -11067, -11055, -11052, -11045, -11041, -11038, -11024, -11020, -11019,
        -11018, -11014, -10838, -10832, -10815, -10800, -10790, -10780, -10764, -10587, -10544, -10533, -10519, -10331, -10329, -10328,
        -10322, -10315, -10309, -10307, -10296, -10281, -10274, -10270, -10262, -10260, -10256, -10254
     };
        #endregion
        #region referenceChinese
        private static string[] referenceChinese = new string[] {
            "吖", "八", "嚓", "哒", "屙", "发", "旮", "铪", "丌", "咔", "垃", "妈", "拿", "噢", "趴", "七",
            "蚺", "仨", "他", "哇", "夕", "丫", "匝"
        };
        #endregion

        #region 五笔
        static string[] wubima = new string[]{
//a
"蔼艾鞍芭茇菝靶蒡苞葆蓓鞴苯荸荜萆蓖蔽薜鞭匾苄菠薄菜蔡苍藏艹草茬茶蒇菖苌臣茌茺莼茈茨苁葱蔟萃靼鞑甙萏荡菪荻蒂东鸫董蔸芏莪苊萼蒽贰藩蕃蘩范匚芳菲匪芬葑芙芾苻茯莩菔甘苷藁戈革葛茛工功攻恭廾巩汞共贡鞲苟菇菰鹳匦邯菡蒿薅荷菏蘅薨荭蕻葫花划萑荒黄茴荟蕙荤劐或获惑藿芨基蒺蕺芰蓟葭荚菅蒹鞯茧荐茳蒋匠艽茭蕉节戒芥藉堇荩靳觐茎荆菁警敬苴鞠鞫菊莒巨苣蕨菌蒈勘戡莰苛恐芤蔻苦蒯匡葵匮蒉莱蓝莨蒗劳勒蕾莉蓠藜苈荔莅莲蔹蓼蔺苓菱茏蒌芦萝荦落荬颟鞔蔓芒茫莽茅茆茂莓萌甍瞢蒙蘼苗鹋藐蔑苠茗摹蘑茉莫蓦某苜募墓幕慕暮艿萘匿廿茑孽蘖欧殴瓯鸥藕葩蒎蓬芘匹苤苹萍叵莆菩葡蒲七萋期欺芪其萁綦蕲芑荠葺葜芊荨芡茜蔷荞鞒巧翘鞘切茄芩芹勤擎檠苘跫銎邛茕蛩区蕖蘧荃颧鹊苒荛惹荏葚戎茸荣蓉鞣茹薷蓐蕤蕊芮若萨散莎芟苫芍苕甚蓍莳世式贳菽蔬薯蒴斯菘薮苏蔌蒜荽荪蓑苔薹萄忒慝藤萜莛葶茼荼菟芄莞菀葳薇苇萎蔚蓊蕹莴卧巫芜芴昔菥熙觋葸蓰匣莶藓苋芗葙巷项萧邪鞋薤芯莘薪荇芎蓄蓿萱靴薛薰荀蕈鸦牙芽雅迓菸蔫芫郾燕鞅尧药医荑颐苡弋艺薏翳茵荫鄞茚英莺茔荥荧莹萤营萦蓥莜莸莠萸芋蓣鸢苑芸蕴匝葬藻赜蘸蔗斟蓁蒸芝芷荮茱苎著茁菹蕞"
//b
,"阿隘阪孢陂陛屮陈丞承蚩耻出除陲聪耽聃阽耵陡队堕耳防阝附陔隔耿孤聒孩函隍隳亟际降阶卩孑卺阱聚孓孔聩联辽聊了陵聆隆陇陋陆勐孟陌陧聂颞聍陪陴聘阡凵取娶孺阮陕隋随祟隧孙陶粜陀隈隗卫阢隰隙险限陷陉逊阳耶也阴隐隅院陨障阵职陟骘坠孜子陬鄹阻阼"
//c
,"巴畚弁骠驳参骖叉骋驰骢皴迨怠邓叠对怼驸观骇骅欢鸡骥艰骄矜劲刭颈迳驹骏骒垒骊骝驴骡骆马矛蝥蟊瞀牟鍪难能骈骗骐骑巯驱劝逡柔叁毵桑颡骚骟圣驶双厶驷骀台邰炱通驮驼婺骛鹜戏骧骁熊驯验以矣驿甬勇恿又予驭预豫鹬允驵蚤骣骤驻骓驺"
//d
,"砹碍鹌百邦帮磅悲碑辈碚奔泵砭碥髟飙鬓礴布礤厕碴虿厂耖砗辰碜成舂厨础春唇蠢磁蹙存磋厝耷达大砀焘磴砥碲碘碉碟碇硐碓礅趸砘夺厄而鸸砝矾非蜚斐翡奋丰砜酆奉砩尬尴感矸硌耕龚鸪辜古嘏故顾硅磙夯耗厚胡鹕瓠鬟磺灰彗慧耠矶剞髻恝戛硷碱礓耩礁碣兢鬏韭厩厥劂砍磕克刳夸夼矿盔奎髡砬耢耒磊厘历厉励砺砾奁鹩尥鬣磷硫龙砻聋垄耧碌码劢迈硭髦礞面耱奈耐硇碾耨恧耦耙耪匏裴砰硼碰砒破戚奇契砌碛牵硗挈秦磲鬈犬确髯辱三磉砂奢厍砷蜃盛石寿戍耍爽硕厮耜肆碎太态泰碳耥套髫厅砼砣碗万威硪戊矽硒袭硖夏厦咸厢硝硎雄髹戌砉碹压砑研奄厣魇厌砚艳雁餍赝页靥欹硬尢尤友有右郁原愿耘砸在臧仄砟丈磔砧碡砖斫髭耔鬃奏左"
//e
,"爱胺肮膀胞豹膘豳膑脖膊采彩豺肠塍腠脆脞胆貂腚胨胴肚腭肪肥腓肺肤孚服郛脯腑腹尕戤肝肛胳膈肱股臌胍胱虢胲貉肌及胛腱胶脚腈肼胫雎爵胩胯脍腊肋臁脸膦胧胪脶脉貌朦脒觅腼邈膜貊貘肭乃鼐腩脑腻脲脓胖脬胚朋鹏膨脾貔胼脐肷腔且朐肜乳朊脎腮臊彡膻膳胂胜豕受腧甩舜胎肽膛腾滕腆腿豚脱妥腽脘腕肟奚膝燹县腺胁腥胸貅须悬胭腌腰遥繇舀鹞腋胰臆盈媵臃用有腴爰月刖孕脏膪胀胗朕肢胝脂豸膣肿肘逐助肫腙胙"
//f
,"埃霭埯坳坝霸坂雹贲甏孛勃博鹁埠才裁场超朝坼趁城埕墀赤翅亍矗寸埭戴堤觌坻地颠坫垤堞耋动垌都堵堆墩垛二坊霏坟封夫赴垓干坩赶圪塥埂垢彀遘觏毂鼓瞽卦圭规埚过顸邗韩翰壕郝盍赫堠壶觳坏卉恚魂霍击圾赍吉戟霁嘉教劫颉截进井境赳救趄均垲刊堪坎考坷壳坑堀垮块款圹亏逵坤垃老雷塄嫠坜雳墚埒趔霖零酃垅露垆埋霾卖墁耄霉坶南赧垴坭霓辇埝培霈堋彭坯霹埤鼙圮坪坡埔亓圻耆起乾墙謦磬罄求逑裘趋麴去趣却悫壤韧颥丧埽啬霎埏墒垧赦声十埘士示螫霜寺索塌塔坍坛坦塘趟韬替填霆土堍坨顽韦圩违未雯斡圬无坞雾熹喜霞献霰霄孝协馨幸需墟雪埙垭盐堰壹圯埸懿堙垠霪墉雩雨域元垣袁鼋塬远垸越云运韫哉栽载趱增赵者赭真圳震支直埴址志煮翥专趑走"
//g
,"瑷敖獒遨熬聱螯鳌骜鏊班斑甭逼碧表殡丙邴玻逋不残蚕璨曹琛豉敕刺璁琮殂璀歹带殆玳殚到纛玷靛玎豆逗毒蠹顿垩恶噩珥珐玢否麸敷甫副丐鬲亘更珙瑰翮珩瑚琥互画还环璜虺珲惠丌玑墼棘殛夹珈郏颊戋歼柬戬豇瑾晋靓静玖琚珏开珂琨剌来赉赖琅鹂璃逦理吏丽郦琏殓两列烈裂琳玲琉珑璐珞玛麦瑁玫芈灭玟珉末殁囊孬瑙弄琶丕邳琵殍平珀璞妻琦琪琴青琼球璩融瑞卅瑟珊殇事殊束死素速琐瑭忑天忝殄餮吞屯橐瓦歪豌玩琬王玮軎吾五武鹉兀瑕下现刑邢形型顼璇殉琊亚焉鄢严琰殃珧瑶一夷殪瑛璎迂于欤盂瑜与玉瑗殒再瓒遭枣责盏璋珍臻整正政殖至郅致珠赘琢"
//h
,"龅彪卜步睬餐粲柴觇龀瞠眵齿瞅龊雌此鹾眈瞪睇点盯鼎督睹盹丨壑虍虎乩睑睫睛旧龃具遽瞿矍卡瞰瞌肯眍眶睽睐瞵龄卢鸬颅卤虏虑瞒眯眠眄瞄眇瞑眸目睦睨虐盼皮睥瞟频颦颇攴歧虔瞧氍龋觑睿上叔睡瞬瞍眭睢睃忐龆眺瞳凸龌瞎些盱虚眩睚眼眙龈卣虞龉眨砦瞻占战贞睁止瞩卓桌赀觜龇紫訾眦"
//i
,"澳灞浜滗濞汴滨濒波泊渤不沧漕测涔汊潺尝常敞氅潮澈尘沉澄池滁淳淙淬沓淡澹当党凼滴涤滇淀洞渎渡沌沲洱法泛淝沸汾瀵沣浮涪滏尜溉泔澉淦港沟沽汩涫灌光滚海涵汉汗瀚沆濠浩灏河涸泓洪鸿黉鲎滹湖浒沪滑淮洹浣涣漶湟潢辉洄汇浍浑混溷活激汲脊洎济浃尖湔涧渐溅江洚浇湫洁津浸泾酒沮举涓觉浚渴溘喾溃涞濑澜漤滥浪潦涝泐泪漓澧沥溧涟濂潋梁粱劣洌淋泠溜浏流鎏泷漏泸渌滤漉潞滦沦泺洛漯满漫漭泖没湄浼懑汨泌沔湎淼渺泯溟沫漠沐淖泥溺涅泞浓沤派湃潘泮滂泡沛湓澎淠漂泼婆濮浦溥瀑沏柒漆淇汔汽泣洽潜浅溱沁清泅渠雀染溶濡汝洳溽润洒涩沙裟鲨潸汕裳赏尚少潲涉滠深沈渖渗渑省湿淑沭漱澍涮氵水澌汜泗淞溲涑溯濉娑挲溻汰滩潭汤堂棠溏淌烫涛滔洮逃淘鼗涕添汀潼涂湍沱洼湾汪沩涠潍洧渭温汶涡沃渥污浯鋈汐浠淅溪洗涎湘削消逍潇淆小肖泄泻渫瀣兴汹溴洫溆漩泫渲学泶洵浔汛涯淹湮沿演滟泱洋漾耀液漪沂溢洇淫滢潆瀛泳涌油游淤渝渔浴誉渊沅源瀹澡泽渣沾澶湛漳涨掌沼兆浙浈汁治滞洲洙潴渚注涿浊浞濯淄滋滓渍"
//j
,"暧暗昂蚌暴蝙晡螬蝉蟾昌畅晁晨蛏螭匙虫蜍蝽旦刂戥电蝶蚪蛾遏蜂蚨蜉蝠蝮旰杲蛤虼蚣蛄蛊归晷炅蝈果蜾蚶晗旱蚝昊颢曷蚵虹蝴蝗蟥晃晖蛔晦蟪夥蠖虮蛱坚监鉴蛟蚧紧晶景颗蝌旷暌蝰昆蛞旯蜡览螂蜊里蛎蠊晾量临蛉蝼螺蟆蚂螨曼蟒蛑昴冒昧虻盟蜢蠓冕蠛明暝螟蝻曩蛲昵暖蟠螃蟛蚍蜱螵曝蛴蜞蜣螓蜻晴蚯虬蝤蛆蛐蠼蜷蚺日蝾蠕蚋晒蟮晌蛸蛇申肾晟师时是暑曙竖墅帅蟀蛳螋遢昙螗螳剔题蜩蜓蜕暾蛙蜿晚旺韪蚊蜗蜈晤晰蜥螅蟋曦虾暇暹贤显蚬蟓晓歇蝎昕星勖煦暄曛蚜蜒晏蛘曜野曳晔蚁易蜴蚓蝇影映蛹蚰蝣蚴禺愚蝓昱遇蜮螈曰昀晕早昃蚱蟑昭照蜘蛭蛛蛀最昨"
//k
,"吖啊嗄哎唉嗳嗌嗷叭吧跋呗趵嘣蹦吡鄙哔跸别啵踣跛卟哺嚓踩嘈噌蹭躔唱嘲吵嗔呈逞吃哧嗤踟叱踌躇蹰啜嘬踹川喘串吹踔呲蹴蹿啐蹉哒嗒呆呔啖叨蹈噔蹬嘀嗲踮叼吊跌喋蹀叮啶咚嘟吨蹲咄哆踱哚跺呃鄂鹗颚蹯啡吠吩唪呋趺跗呒咐嘎噶嗝跟哏哽咕呱剐咣贵跪呙哈嗨喊嚆嗥嚎号呵喝嗬嘿哼哄喉吼呼唿唬哗踝唤患咴哕喙嚯叽咭唧跻戢哜跽跏趼践踺跤叫噍喈嗟噤啾咀踽距踞鹃噘噱蹶嚼咔咖喀咳嗑啃吭口叩哭跨哙哐喹跬喟啦喇啷唠叻嘞喱哩呖唳跞踉嘹咧躐啉躏另呤咯咙喽噜路鹭吕骂唛吗嘛咪嘧黾喵咩鸣哞哪喃囔呶呐呢嗯啮嗫蹑咛哝喏噢哦呕趴啪哌蹒咆跑呸喷嘭噼啤蹁嘌品噗蹼嘁蹊器遣呛跄跷嗪噙吣嚷蹂嚅噻嗓唼啥跚哨呻哂史嗜噬唰吮顺嗍嗽咝嘶嗣嗖嗾嗉虽唆嗦唢趿踏蹋跆叹饕啕踢啼蹄嚏跳听嗵吐跎鼍唾哇唯味喂吻嗡喔呜吴唔吸唏嘻呷吓跹跣响哓嚣哮啸躞兄咻嘘嗅喧勋呀哑咽唁吆咬噎叶咿噫咦遗呓邑喑吟吲嘤郢哟唷喁咏踊呦吁喻员跃郧咂咱唣噪躁啧吒咋哳喳咤啁吱跖踯只趾踬中忠盅踵咒躅嘱啭啄踪足躜嘴唑"
//l
,"黯罢办畀边黪车畴黜辍辏黩囤轭恩罚畈罘辐辅罡哿轱罟固轨辊国贺黑轰轷囫回畸羁辑加迦袈甲驾架囝轿较界轲困罱累罹力轹詈连辆辚囹轳辂辘略囵轮罗逻皿墨默囡男嬲畔毗罴圃畦黔堑椠轻圊黥囚黢圈辁畎轫软轼输署蜀思四田畋町图团疃畹辋囗围畏胃辖黠勰轩鸭罨轺黟轶因黝囿圄圉园圆辕圜暂錾罾轧斩辗罩辄辙轸畛轵轾置轴转辎罪"
//m
,"岸盎凹岜败贝崩髀贬飑飚髌财册岑崇帱遄赐崔嵯丹嶝迪骶巅典雕岽峒髑赌朵剁峨帆幡凡贩风峰凤幅幞赋赙赅冈刚岗骼岣购鹘骨崮刿崞帼骸骺岵凰幌贿岌几嵴觊岬见贱峤骱巾赆冂迥飓崛峻凯剀髁岢崆骷髋贶岿崃岚崂嶙岭髅嵝赂幔峁帽嵋岷内帕赔帔岐崎屺岂髂岍嵌峭赇曲岖冉嵘肉山删赡赊嵊殳赎兕崧嵩飕夙髓岁炭赕贴帖同彤骰崴网罔巍帏帷嵬幄峡岘崤岫峋岈崖崦岩央鸯崾贻嶷屹峄婴罂鹦由邮嵛屿峪崽赃则帻贼赠崭帐账嶂幛赈峥帧帙帜峙周胄贮颛赚幢嵫"
//n
,"懊悖鐾必愎辟壁嬖避臂璧襞忭擘檗怖惭惨恻层孱忏羼惝怅怊忱迟尺忡憧惆丑怵憷怆戳悴翠忖怛惮蛋忉导悼惦殿刁懂恫惰屙愕发飞悱愤怫改敢怪惯憨悍憾恨恒惚怙怀慌惶恍恢悔屐己忌悸届尽惊憬居局剧惧屦恺慨忾慷尻恪快悝愦愧悃懒愣怜懔鹨戮屡履买慢忙眉鹛懵乜民悯愍恼尼怩尿忸懦怄怕爿怦劈屁甓譬屏恰悭慊戕悄憔愀怯惬情屈悛慑慎尸虱屎恃收书疏刷司巳忪悚愫屉悌惕恬恸屠臀惋惘惟尾尉慰屋忤怃悟惜犀习屣遐屑懈忄心忻惺性悻胥恤恂迅巽疋恹怏怡乙已以忆异怿羿悒翌翼慵忧愉羽悦恽愠熨奘憎翟展怔咫忮昼属惴怍"
//o
,"粑爆焙煸灬炳灿糙焯炒炽炊糍粗粹灯断煅炖烦燔粉粪烽黻黼糕焓焊烘糇烀煳糊焕煌烩火糨烬粳精炯炬爝糠炕烤烂烙类粒粝炼粮燎料粼遴熘娄炉熳煤焖迷米敉糯炮粕炝糗炔燃熔糅糁煽剡熵烧炻数烁燧郯糖烃煺烷煨为炜焐烯粞熄籼燮糈煊炫烟炎焰焱炀烊业邺烨熠煜燠糌糟凿灶燥炸粘黹烛炷灼籽粽"
//p
,"安案袄宝褓被褙裨窆褊裱宾补察衩禅宸衬裎褫宠初褚穿窗辶祠窜褡裆宕祷定窦裰额祓袱福富袼割宫寡褂官冠宄害寒罕褐鹤宏祜寰宦逭豁祸寂寄家袷裥謇蹇窖衿襟窘究裾窭军皲窠客裉空寇窟裤宽窥褴牢礼帘裢裣寥寮窿禄褛裸袂寐祢冖宓密幂蜜宀冥寞衲宁甯农袢襻袍祁祈祺骞搴褰襁窍窃寝穷穹祛裙禳衽容冗襦褥塞赛衫社神审实礻视室守祀宋宿邃它袒裼祧窕突褪袜剜完宛窝寤穸禧禊祆宪祥宵写袖宣穴窨宴窑窈衤宜寅廴宥窬宇窳寓裕冤郓灾宰宅窄寨褶这祯鸩之祗祉窒冢宙祝窀禚字宗祖祚"
//q
,"锕锿铵犴钯鲅钣镑勹包饱鲍狈钡锛狴铋鳊镖镳鳔镔饼钵饽钹铂钸钚猜馇锸猹镲钗馋镡铲猖鲳鬯钞铛铖鸱饬铳刍锄雏触舛钏璠锤匆猝镩锉错岛锝镫镝狄氐邸甸钿鲷钓铞鲽钉锭铥兜钭独镀锻镦钝多铎锇饿锷鳄儿鲕尔迩饵铒钒犯饭钫鲂鲱狒镄鲼锋孵凫匐负鲋鳆钆钙钢镐锆镉铬鲠觥勾钩狗够觚钴锢鲴鳏馆盥犷逛龟鲑鳜鲧锅猓铪狠訇猴忽狐斛猢鹱铧猾獾郇锾奂鲩鳇昏馄锪钬镬饥急鲚鲫镓铗钾鲣锏饯键鲛角狡饺铰桀鲒解钅金锦馑鲸獍镜久灸狙锔句钜锯镌锩狷觖獗镢钧锎铠锴钪铐钶锞铿狯狂馈锟鲲铼镧狼锒铹铑乐鳓镭狸鲡锂鲤鳢猁鲢镰链獠镣钌猎鳞铃鲮留遛馏镏锍镂鲈鲁镥铝卵锊猡锣镙犸馒鳗镘猫锚卯铆贸猸镅镁钔猛锰猕免勉名铭馍镆钼镎钠馕铙猱馁铌猊鲵鲇鲶鸟袅镊镍狞狃钮钕锘刨狍锫铍鲆钋钷铺匍镤镨鳍钎铅钤钱钳欠锖锵镪锹锲钦锓卿鲭鳅犰劬鸲铨犭然饶饪狨铷锐鳃馓鳋色铯杀刹铩煞钐鳝觞勺猞狮鲺饣蚀鲥氏饰铈弑狩铄锶饲馊锼稣觫狻狲飧锁铊獭鳎鲐钛锬钽铴镗饧铽锑逖鲦铫铁铤铜钍兔饨鸵外危猥鲔猬刎我乌邬钨勿夕希郗欷锡玺铣饩郄狎狭锨鲜猃馅镶饷象枭销獬邂蟹锌鑫猩凶匈馐锈铉镟鳕獯旬鲟爻肴鳐铘猗铱饴钇刈逸镒镱铟狺银夤饮印迎镛鳙犹铀鱿铕鱼狳馀饫狱钰眢鸳猿怨钥钺匀狁锃铡詹獐钊锗针镇争狰钲铮炙觯钟锺皱猪铢橥铸馔锥镯锱鲻邹鲰镞钻鳟"
//r
,"挨捱皑氨揞按翱拗扒捌拔魃把掰白捭摆拜扳搬扮拌报抱卑鹎拚摈兵摒拨播帛搏捕擦操插搽拆掺搀抄扯掣撤抻撑魑持斥抽搐搋揣氚捶撺摧搓撮挫措搭打担掸氮挡氘捣的抵掂垫掉迭瓞揲氡抖盾遁掇扼摁反返氛缶扶拂氟抚拊擀缸皋搞搁搿拱瓜挂拐掼罐皈鬼掴氦捍撖撼皓后逅护换擐皇遑挥攉挤掎技搛拣捡挢皎搅敫接揭拮捷斤近揪拘掬拒据捐撅抉掘攫捃揩看扛抗拷氪控抠扣挎揆魁捆扩括拉拦揽捞擂魉撩撂捩拎拢搂撸掳氯掠抡捋摞魅扪描抿摸抹拇捺氖攮挠拟拈年捻撵捏拧牛扭挪搦爬拍排乓抛抨捧批披郫擗氕撇拼乒皤迫魄掊扑颀气掐扦掮抢撬擒揿氢氰丘邱泉缺攘扰热扔揉撒搡搔扫擅捎摄失拾势拭逝誓手扌授抒摅摔拴搠撕搜擞损所挞抬摊探搪掏提掭挑挺捅投抟推托拖拓挖挽皖魍挝握捂舞罅氙掀魈挟携撷卸欣擤揎踅押氩揠掩扬氧邀摇揶掖揖抑挹殷氤撄拥揄援掾岳氲拶攒皂择揸扎摘搌招找蜇折哲蛰摺振挣拯卮执絷摭指制质挚贽掷鸷朱邾拄抓爪拽撰撞拙捉擢揍攥撙"
//s
,"桉柏板梆榜棒杯本杓标彬槟柄醭材槽杈查槎檫郴榇柽枨酲橙酬樗橱杵楮楚椽棰槌椿醇枞楱酢醋榱村档柢棣丁酊顶栋椟杜椴樊梵枋榧酚棼焚枫桴覆概杆柑酐橄杠槔槁哥歌格根梗枸构酤梏棺桄柜桂棍椁醢酣杭核桁横槲醐桦槐桓桧机极楫枷贾枧检楗槛椒酵醮杰槿禁柩桕椐桔橘榉醵鄄桷橛楷栲柯棵可枯酷框醌栝栏婪榄榔醪栳酪檑酹棱楞李醴枥栎栗楝椋林檩柃棂榴柳栊楼栌橹麓榈椤杩懋枚梅楣酶檬梦醚棉杪酩模木柰楠酿柠杷攀醅配棚枇剽飘瓢票榀枰朴栖桤槭棋杞枪樯橇桥樵檎楸权醛榷桡榕枘森杉梢椹酾柿枢梳术述树栓松酥粟酸榫桫梭榻酞覃檀樘醣桃梯醍梃桐酮桶酴柁酡椭柝枉桅梧杌西析皙樨醯檄柙酰相想橡枵校楔械榍榭醒杏朽栩醑酗楦醺桠檐酽杨样杳要椰酏椅樱楹柚酉榆橼樾酝楂札栅榨栈樟杖棹柘桢甄榛枕枝栀植枳酯栉桎酎株槠杼柱桩椎酌梓棕醉樽柞"
//t
,"矮岙奥笆稗般版舨备惫笨鼻彼秕笔舭币筚箅篦笾秉舶箔簸簿舱艚策长徜彻称乘惩程秤笞篪彳艟愁稠筹臭处舡船囱垂辞徂簇汆篡毳矬笪答待箪稻得德簦等籴敌笛第簟牒丢冬篼牍犊笃短簖躲舵鹅乏筏番翻繁彷舫篚逢稃符复馥竿秆筻睾篙稿告郜舸各躬篝笱箍牯鹄牿刮鸹乖管簋鼾航禾和很衡篌後乎笏徊徨篁簧徽秽积笄嵇犄箕稽笈籍季稷笳稼笺犍笕简舰毽箭矫徼秸街筋径咎矩榘犋筠犒靠科稞箜筘筷筐篑徕籁篮稂梨犁黎篱黧利笠篥笼篓舻簏氇稆律乱箩雒毛牦么每艨艋秘秒篾敏鳘秣毪牡牧穆黏臬衄筢徘牌盘磐逄篷片犏篇丿牝鄱笸攵氆乞迄憩千迁愆签箝乔箧箐筇秋鼽躯衢筌穰壬稔入箬穑歃筛舢稍筲艄舌射身矧升生牲笙甥眚剩矢适舐释筮艏秫黍税私笥艘簌算穗笋毯躺特甜舔条笤廷艇筒透秃徒颓乇箨往逶微委艉魏稳我午迕牾务物息牺悉稀舾徙系先舷衔筅香箱向箫筱笑囟衅行秀徐选血熏循徇衙延筵衍秧徉夭徭迤移舣役劓胤牖釉竽禹御毓箢粤簪昝赞造迮笮舴箦怎齄乍毡笊箴稹征筝徵知夂秩智稚雉舯螽种重舟籀竹竺舳筑箸篆秭笫自租纂"
//u
,"癌疤瘢癍半瓣北邶背迸闭敝痹弊辨辩辫瘭憋鳖蹩瘪冫冰并病部瓿差瘥产阐冁阊闯痴啻瘛冲瘳疮疵瓷慈鹚次凑瘁痤瘩单郸瘅疸盗道羝弟帝递癫奠癜凋疔冻斗痘端兑阏阀痱疯冯盖疳赣戆羔疙阁羹痼关闺衮馘阂阖痕闳瘊冱痪豢癀阍疾瘠冀痂瘕间兼煎鹣减剪翦姜将浆奖桨酱交郊疖竭羯疥净痉竞竟靖阄疚疽蠲卷桊眷决竣阚闶疴况夔阃阔瘌辣癞兰阑阆痨冷立疠疬痢凉疗冽凛凌羚瘤六癃瘘闾瘰美门闷闵闽瘼闹疒逆凝疟判叛旁疱疲痞癖瞥瓶剖普凄前歉羌羟妾亲酋遒癯阒拳痊券瘸阕阙闰飒瘙痧闪疝善鄯商韶首兽瘦闩朔槊凇竦送塑遂羧闼瘫痰羰疼誊鹈剃阗童痛头闱痿瘟闻阌问痦羲阋闲痫鹇冼羡翔鲞效辛新歆羞痃癣丫痖阉闫阎颜兖彦羊疡养痒恙冶痍疫益翊意瘗毅癔音瘾瘿痈疣猷瘀瘐阈阅韵曾甑闸痄瘵站章鄣彰瘴疹郑症痔痣瘃疰妆装丬壮状准着兹咨姿资孳粢恣总尊遵"
//v
,"嫒媪妣婢婊剥姹婵娼嫦巢媸巛妲逮刀嫡娣妒娥婀发妨妃鼢妇旮艮媾姑妫好毁婚姬即嫉彐妓既暨嫁奸建姣娇剿婕她姐妗婧鸠九臼舅娟君郡垦恳馗邋姥嫘娌隶灵录逯妈嬷媒妹媚娩妙嫫姆那娜奶嫩妮娘肀妞奴孥驽努弩胬怒女媲嫖姘嫔娉嫱群娆忍刃妊如嫂姗嬗劭邵娠婶始姝鼠恕孀妁姒叟肃帑迢婷退娃娲丸婉娓鼯妩嬉鼷媳舄娴嫌姓旭婿絮寻巡娅嫣妍鼹妖姚姨姻尹邕鼬妤臾舁娱聿妪媛杂甾嫜召妯帚姊"
//w
,"俺傲八爸佰颁伴傍煲保堡倍坌俾便傧伯仓伧侧岔侪伥偿倡侈傺仇俦雠储传创从丛促爨催傣代岱贷袋黛儋但倒登凳低佃爹仃侗段俄佴伐垡仿分份忿偾俸佛伏俘斧俯釜父付阜傅伽鸽个公供佝估谷倌癸刽含颔合何盒颌侯候华化会伙货佶集伎偈祭佳价假俭件剑牮健僭僵焦僬鹪佼侥介借今仅儆僦俱倨倦隽倔俊佧龛侃伉倥侉侩郐傀佬仂儡俚例俐俪傈俩敛僚邻赁伶瓴翎领令偻侣仑伦倮们命侔仫拿倪伲你念您佞侬傩偶俳佩盆仳僻偏贫俜凭仆企仟佥倩戗劁侨俏侵衾禽倾俅全人亻仁仞任恁仍儒偌仨伞僧傻伤畲佘舍伸什食使仕侍售倏舒毹伺似俟怂耸颂俗僳隼他贪倘傥体倜佻停仝佟僮偷途氽佗佤偎伟伪位璺翁瓮倭仵伍侮兮翕僖歙侠仙像偕斜信休修鸺叙儇伢俨偃佯仰爷伊依仪倚亿仡佚佾佣俑优攸悠佑侑余俞逾觎舆伛俣欲鹆愈龠债仉仗侦侄值仲众侏伫住隹追倬仔偬俎佐作坐做"
//x
,"绊绑鸨绷匕比毕毖毙弼编缏缤缠弛绸绌纯绰绐弹缔缎缍纺绯费纷缝弗绂绋艴缚绀纲缟纥给绠弓缑贯绲绗弘红弧缳缓幻绘缋绩缉畿级纪继缄缣缰疆绛犟绞缴皆结缙经弪纠绢绝缂绔纩缆缧缡蠡练缭绫绺缕绿纶络缦弥弭糸绵缅缈缗缪母纳纽辔纰缥绮缱强缲顷绻绕纫绒缛弱缫纱缮绱绍绅绳绶纾纟丝鸶缌绥缩绦绨缇统彖纨绾维纬纹毋细纤弦线乡缃飨绡缬绁绣绪续绚幺疑彝绎缢肄引缨颍颖幽幼纡鬻缘约纭缯绽张缜织旨纸彘终粥纣绉缀缒缁综纵组缵"
//y
,"哀庵谙廒鏖谤褒庇庳扁卞变遍斌禀亳诧谗廛谄颤昶谌谶诚充床鹑词诞谠诋底谛店调谍订读度憝敦讹谔方邡房访放扉诽废讽府腐讣该高膏诰庚赓诟诂雇诖广庋诡郭裹亥颃毫豪诃劾亨讧户戽扈话肓谎诙麾讳诲诨讥迹齑麂计记剂肩谫谏讲讦诘诫谨廑京旌扃就鹫讵诀谲麇康亢颏刻课库诓诳邝廓谰斓郎廊朗羸诔离戾廉娈恋良亮谅廖麟廪吝刘旒庐鹿旅膂率孪峦挛栾鸾脔銮论蠃麻蛮谩邙盲旄袤氓谜糜縻麋靡谧庙谬谟麽摩磨魔谋亩讷旎诺讴庞庖旆烹庀翩谝评裒谱齐旗麒启綮讫弃谦谴敲谯诮请庆诎诠瓤让认讪扇设麝诜谂诗施识市试谥孰塾熟庶衰谁说讼诵诉谡谇谈谭唐讨亭庭亠庹弯亡妄忘望为诿谓文紊诬庑误诶席襄详庠享谐亵谢廨庥许诩序畜谖玄旋谑询训讯讶讠言谚谳谣夜谒衣诒旖义议亦译诣奕弈谊裔应膺鹰嬴赢庸雍壅饔永诱於谀语庾育谕谮诈斋旃谵诏肇遮谪这鹧诊证诤衷州诌诛诸丶主麈庄谆诼谘诹卒族诅座"};

        #endregion

        /// <summary>
        /// 获取全拼
        /// </summary>
        /// <param name="hzString">字符串</param>
        /// <returns></returns>
        public static string GetPinYin(string hzString)
        {
            return GetPinYinWithSplit(hzString, char.MinValue);
        }

        /// <summary>
        /// 获取全拼 按指定字符隔开,默认为空格
        /// </summary>
        /// <param name="hzString">字符串</param>
        /// <param name="splitChar">分隔符,默认为空格</param>
        /// <returns></returns>
        public static string GetPinYinWithSplit(string hzString, char splitChar = ' ')
        {
            if (string.IsNullOrEmpty(hzString)) return string.Empty;
            var schar = splitChar == char.MinValue ? "" : splitChar.ToString();
            byte[] array = new byte[2];
            StringBuilder pySB = new StringBuilder();
            int chrAsc = 0;
            int i1 = 0;
            int i2 = 0;
            char[] noWChar = hzString.ToCharArray();
            for (int j = 0; j < noWChar.Length; j++)
            {
                if (regex.IsMatch(noWChar[j].ToString()))
                {
                    array = Encoding.Default.GetBytes(noWChar[j].ToString());
                    i1 = array[0];
                    i2 = array[1];
                    chrAsc = ((i1 * 0x100) + i2) - 0x10000;
                    if ((chrAsc > -2050) || (chrAsc < -20319))
                    {
                        pySB.Append(noWChar[j]);
                    }
                    else if (chrAsc <= -10247)
                    {
                        for (int aPos = 11; aPos >= 0; aPos--)
                        {
                            int aboutPos = aPos * 0x21;
                            if (chrAsc >= pyValue[aboutPos])
                            {
                                for (int i = aboutPos + 0x20; i >= aboutPos; i--)
                                {
                                    if (pyValue[i] <= chrAsc)
                                    {
                                        pySB.Append(schar).Append(pyName[i]);
                                        break;
                                    }
                                }
                                break;
                            }
                        }
                    }
                    else
                    {
                        int pos = Array.IndexOf<string>(otherChinese, noWChar[j].ToString());
                        if (pos != -1M)
                        {
                            pySB.Append(schar).Append(otherPinYin[pos]);
                        }
                    }
                }
                else
                {
                    pySB.Append(noWChar[j].ToString());
                }
            }
            return pySB.ToString().TrimStart(new char[] { splitChar });
        }

        /// <summary>
        /// 获取拼音首字母
        /// </summary>
        /// <param name="hzString">字符串</param>
        /// <returns></returns>
        public static string GetFirstPinYin(string hzString)
        {
            if (string.IsNullOrEmpty(hzString)) return string.Empty;
            byte[] array = new byte[2];
            StringBuilder pySB = new StringBuilder();
            int chrAsc = 0;
            int i1 = 0;
            int i2 = 0;
            char[] noWChar = hzString.ToCharArray();
            for (int j = 0; j < noWChar.Length; j++)
            {
                if (regex.IsMatch(noWChar[j].ToString()))
                {
                    array = Encoding.Default.GetBytes(noWChar[j].ToString());
                    i1 = array[0];
                    i2 = array[1];
                    chrAsc = ((i1 * 0x100) + i2) - 0x10000;
                    if ((chrAsc > -2050) || (chrAsc < -20319))
                    {
                        pySB.Append(noWChar[j]);
                    }
                    else
                    {
                        string s = noWChar[j].ToString();
                        if (Array.IndexOf<string>(referenceChinese, s) == -1)
                        {
                            string[] newArray = new string[] {
                            "吖", "八", "嚓", "哒", "屙", "发", "旮", "铪", "丌", "咔", "垃", "妈", "拿", "噢", "趴", "七",
                            "蚺", "仨", "他", "哇", "夕", "丫", "匝", ""
                         };
                            newArray[0x17] = s;
                            Array.Sort<string>(newArray);
                            int i = Array.IndexOf<string>(newArray, s);
                            pySB.Append(pinYinArray[i - 1]);
                        }
                        else
                        {
                            int i = Array.IndexOf<string>(referenceChinese, s);
                            pySB.Append(pinYinArray[i]);
                        }
                    }
                }
                else
                {
                    pySB.Append(noWChar[j].ToString());
                }
            }
            return pySB.ToString().ToUpper();
        }

        /// <summary>
        /// 获取五笔码
        /// </summary>
        /// <param name="hzString">字符串</param>
        /// <returns></returns>
        public static string GetWuBiMa(string hzString)
        {
            if (string.IsNullOrEmpty(hzString)) return string.Empty;
            var wbm = new StringBuilder();
            foreach (var c in hzString)
            {
                int i = 0;
                for (; i < wubima.Length; i++)
                {
                    if (wubima[i].Contains(c.ToString()))
                    {
                        wbm.Append((char)(65 + i));
                        break;
                    }
                }
                if (i >= wubima.Length) wbm.Append(c);
            }
            return wbm.ToString().ToUpper();
        }
    }
}
