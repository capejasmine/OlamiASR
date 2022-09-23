using System;
using System.Collections.Generic;
/// <summary>
/// Json 数据存储
/// </summary>
[Serializable]
public static class Json_ZH
{

    #region 基础模块返回数据

    /// <summary>
    /// 详细描述
    /// </summary>
    [Serializable]
    public class Desc_obj
    {
        //返回值
        public string result;

        //状态
        public int status;

        //类型
        public string type;
    }

    /// <summary>
    /// 语言项
    /// </summary>
    [Serializable]
    public class NliItem
    {
        public Desc_obj desc_obj;
        //执行的模块  比如 日期（date）或者 闲聊（nonsense）
        public string type;
    }

    /// <summary>
    /// 自然语言
    /// </summary>
    [Serializable]
    public class Data
    {
        public List<NliItem> nli;
    }

    /// <summary>
    /// 根节点
    /// </summary>
    [Serializable]
    public class Root
    {
        //成功：ok ，失败：error
        public string status;
        //语义理解结果的 JSON 数据
        public Data data;
    }

    #endregion

    #region 故事模块返回数据  joke story  讲个故事吧

    /// <summary>
    /// 故事模块
    /// </summary>
    [Serializable]
    public class Desc_obj02
    {
        /// <summary>
        /// 基础返回值
        /// </summary>
        public string result;
        /// <summary>
        /// 故事名称
        /// </summary>
        public string name;
        /// <summary>
        /// 所属类型
        /// </summary>
        public string type;

        public int status;
    }
    [Serializable]
    public class Data_objItem02
    {
        /// <summary>
        /// 具体故事内容
        /// </summary>
        public string content;
    }
    [Serializable]
    public class NliItem02
    {
        public Desc_obj02 desc_obj;

        public List<Data_objItem02> data_obj;

        public string type;
    }
    [Serializable]
    public class Data02
    {

        public List<NliItem02> nli;
    }
    [Serializable]
    public class Root02
    {
        //成功：ok ，失败：error
        public string status;
        //语义理解结果的 JSON 数据
        public Data02 data;
    }

    #endregion

    #region 百度百科模块返回数据  baike   什么是海豹

    [Serializable]
    public class Desc_obj03
    {
        /// <summary>
        /// 基础返回值
        /// </summary>
        public string result;
        /// <summary>
        /// 
        /// </summary>
        public int status;
    }
    [Serializable]
    public class Data_objItem03
    {
        /// <summary>
        /// 
        /// </summary>
        public List<int> highlight;


        /// <summary>
        /// 简略信息
        /// </summary>
        public List<string> field_value;

        /// <summary>
        /// 具体描述
        /// </summary>
        public string description;

        /// <summary>
        /// 印象照片网址
        /// </summary>
        public string photo_url;

        public string type;

        /// <summary>
        /// 类别列表
        /// </summary>
        public List<string> categorylist;

        /// <summary>
        /// 字段名称
        /// </summary>
        public List<string> field_name;
    }
    [Serializable]
    public class NliItem03
    {
        /// <summary>
        /// 
        /// </summary>
        public Desc_obj03 desc_obj;
        /// <summary>
        /// 
        /// </summary>
        public List<Data_objItem03> data_obj;
        /// <summary>
        /// 
        /// </summary>
        public string type;
    }
    [Serializable]
    public class Data03
    {
        /// <summary>
        /// 
        /// </summary>
        public List<NliItem03> nli;
    }
    [Serializable]
    public class Root03
    {
        /// <summary>
        /// 
        /// </summary>
        public string status;
        /// <summary>
        /// 
        /// </summary>
        public Data03 data;

    }

    #endregion


    #region 选择模块 返回数据  selection  新闻联播在哪

    [Serializable]
    public class Desc_obj04
    {
        /// <summary>
        /// 
        /// </summary>
        public string type;
        /// <summary>
        /// 
        /// </summary>
        public int status;
    }
    [Serializable]
    public class Data_objItem04
    {
        /// <summary>
        /// 中国
        /// </summary>
        public string name;
        /// <summary>
        /// [文化, 书籍]
        /// </summary>
        public string desc;
    }
    [Serializable]
    public class NliItem04
    {
        /// <summary>
        /// 
        /// </summary>
        public Desc_obj04 desc_obj;
        /// <summary>
        /// 
        /// </summary>
        public List<Data_objItem04> data_obj;
        /// <summary>
        /// 
        /// </summary>
        public string type;
    }
    [Serializable]
    public class Data04
    {
        /// <summary>
        /// 
        /// </summary>
        public List<NliItem04> nli;
    }
    [Serializable]
    public class Root04
    {
        /// <summary>
        /// 
        /// </summary>
        public string status;
        /// <summary>
        /// 
        /// </summary>
        public Data04 data;
    }

    #endregion


    #region 菜肴模块返回数据 cooking  糖醋排骨怎么做

    [Serializable]
    public class Desc_obj05
    {
        /// <summary>
        /// 菜肴返回值
        /// </summary>
        public string result;
        /// <summary>
        /// 
        /// </summary>
        public int status;
    }
    [Serializable]
    public class Data_objItem05
    {
        /// <summary>
        /// 
        /// </summary>
        public string name;
        /// <summary>
        /// 红烧排骨
        /// </summary>
        public string content;
    }
    [Serializable]
    public class NliItem05
    {
        /// <summary>
        /// 
        /// </summary>
        public Desc_obj05 desc_obj;
        /// <summary>
        /// 
        /// </summary>
        public List<Data_objItem05> data_obj;
        /// <summary>
        /// 
        /// </summary>
        public string type;
    }
    [Serializable]
    public class Data05
    {
        /// <summary>
        /// 
        /// </summary>
        public List<NliItem05> nli;
    }
    [Serializable]
    public class Root05
    {
        /// <summary>
        /// 
        /// </summary>
        public string status;
        /// <summary>
        /// 
        /// </summary>
        public Data05 data;
    }


    #endregion


    #region 搜索模块返回数据  openweb  搜索西瓜

    [Serializable]
    public class Desc_obj06
    {

        public string result;
        /// <summary>
        /// 
        /// </summary>
        public string type;
        /// <summary>
        /// 
        /// </summary>
        public int status;
    }
    [Serializable]
    public class Data_objItem06
    {
        /// <summary>
        /// 
        /// </summary>
        public string url;
    }
    [Serializable]
    public class NliItem06
    {
        /// <summary>
        /// 
        /// </summary>
        public Desc_obj06 desc_obj;
        /// <summary>
        /// 
        /// </summary>
        public List<Data_objItem06> data_obj;
        /// 
        /// </summary>
        public string type;
    }
    [Serializable]
    public class Data06
    {
        /// <summary>
        /// 
        /// </summary>
        public List<NliItem06> nli;
    }
    [Serializable]
    public class Root06
    {
        /// <summary>
        /// 
        /// </summary>
        public string status;
        /// <summary>
        /// 
        /// </summary>
        public Data06 data;
    }

    #endregion


    #region 股票模块返回数据  stock  中国石油股票

    [Serializable]
    public class Desc_obj07
    {
        /// <summary>
        /// 主人，为你找到中国石油的最新股票行情。
        /// </summary>
        public string result;
        /// <summary>
        /// 
        /// </summary>
        public int status;
    }
    [Serializable]
    public class Data_objItem07
    {
        /// <summary>
        /// 股票数量
        /// </summary>
        public string amount;
        /// <summary>
        /// 最低交易
        /// </summary>
        public string cur_price;
        /// <summary>
        /// 今开
        /// </summary>
        public string price_start;
        /// <summary>
        /// 意图
        /// </summary>
        public string intent;
        /// <summary>
        /// 最低
        /// </summary>
        public string price_low;
        /// <summary>
        ///  变化率
        /// </summary>
        public string change_rate;
        /// <summary>
        /// 成交量
        /// </summary>
        public string volume;
        /// <summary>
        /// 变化量
        /// </summary>
        public string change_amount;
        /// <summary>
        /// 历史
        /// </summary>
        public int is_history;
        /// <summary>
        /// 昨收
        /// </summary>
        public string price_end;
        /// <summary>
        /// 股票名称
        /// </summary>
        public string name;
        /// <summary>
        /// 股票代码
        /// </summary>
        public string id;
        /// <summary>
        /// 查询时间
        /// </summary>
        public string time;
        /// <summary>
        /// 最高点
        /// </summary>
        public string price_high;
        /// <summary>
        /// 受喜爱的
        /// </summary>
        public string favorite;
    }
    [Serializable]
    public class NliItem07
    {
        /// <summary>
        /// 
        /// </summary>
        public Desc_obj07 desc_obj;
        /// <summary>
        /// 
        /// </summary>
        public List<Data_objItem07> data_obj;
        /// <summary>
        /// 
        /// </summary>
        public string type;
    }
    [Serializable]
    public class Data07
    {
        /// <summary>
        /// 
        /// </summary>
        public List<NliItem07> nli;
    }
    [Serializable]
    public class Root07
    {
        /// <summary>
        /// 
        /// </summary>
        public string status;
        /// <summary>
        /// 
        /// </summary>
        public Data07 data;
    }

    #endregion


    #region 电视节目模块返回数据  tvprogram 电视节目 CCTV1

    [Serializable]
    public class Data_objItem08
    {
        /// <summary>
        /// 
        /// </summary>
        public int highlight;
        /// <summary>
        /// 节目名字
        /// </summary>
        public string name;
        /// <summary>
        /// 播出时间
        /// </summary>
        public string time;

    }
    [Serializable]
    public class Desc_obj08
    {
        /// <summary>
        /// 请问你想要看哪个频道？
        /// </summary>
        public string result;
        /// <summary>
        /// 
        /// </summary>
        public string type;
        /// <summary>
        /// 
        /// </summary>
        public int status;
    }
    [Serializable]
    public class NliItem08
    {
        /// <summary>
        /// 
        /// </summary>
        public Desc_obj08 desc_obj;

        /// <summary>
        /// 
        /// </summary>
        public List<Data_objItem08> data_obj;
        /// <summary>
        /// 
        /// </summary>
        public string type;
    }
    [Serializable]
    public class Data08
    {
        /// <summary>
        /// 
        /// </summary>
        public List<NliItem08> nli;
    }
    [Serializable]
    public class Root08
    {
        /// <summary>
        /// 
        /// </summary>
        public string status;
        /// <summary>
        /// 
        /// </summary>
        public Data08 data;
    }


    #endregion


    #region 天气模块返回数据  weather  上海的天气 

    [Serializable]
    public class Desc_obj09
    {
        /// <summary>
        /// 上海今天小雨转阴，最高温度33℃，最低温度27℃，南风3-4级。
        /// </summary>
        public string result;
        /// <summary>
        /// 
        /// </summary>
        public int status;
    }
    [Serializable]
    public class Data_objItem09
    {
        /// <summary>
        /// 
        /// </summary>
        public int date;
        /// <summary>
        /// 
        /// </summary>
        public int weather_end;
        /// <summary>
        /// 
        /// </summary>
        public int pm25;
        /// <summary>
        /// 上海
        /// </summary>
        public string city;
        /// <summary>
        /// 
        /// </summary>
        public string temperature_low;
        /// <summary>
        /// 
        /// </summary>
        public int weather_start;
        /// <summary>
        /// 
        /// </summary>
        public string temperature_high;
        /// <summary>
        /// 2021年07月29日,小雨转阴,南风3-4级,最高温度33℃,最低温度27℃。
        /// </summary>
        public string description;
        /// <summary>
        /// 
        /// </summary>
        public int is_querying;
        /// <summary>
        /// 
        /// </summary>
        public int real_date;
        /// <summary>
        /// 南风3-4级
        /// </summary>
        public string wind;
    }
    [Serializable]
    public class NliItem09
    {
        /// <summary>
        /// 
        /// </summary>
        public Desc_obj09 desc_obj;
        /// <summary>
        /// 
        /// </summary>
        public List<Data_objItem09> data_obj;
        /// <summary>
        /// 
        /// </summary>
        public string type;
    }
    [Serializable]
    public class Data09
    {
        /// <summary>
        /// 
        /// </summary>
        public List<NliItem09> nli;
    }
    [Serializable]
    public class Root09
    {
        /// <summary>
        /// 
        /// </summary>
        public string status;
        /// <summary>
        /// 
        /// </summary>
        public Data09 data;
    }

    #endregion


    #region 诗集选择模块 poem selection 水调歌头 苏轼
    [Serializable]
    public class Desc_obj10
    {
        /// <summary>
        /// 有下面这些诗文,请从中选出一个,我来读
        /// </summary>
        public string result;
        /// <summary>
        /// 
        /// </summary>
        public string type;
        /// <summary>
        /// 
        /// </summary>
        public int status;
    }
    [Serializable]
    public class Data_objItem10
    {
        /// <summary>
        /// 水调歌头
        /// </summary>
        public string poem_name;
        /// <summary>
        /// 苏轼
        /// </summary>
        public string author;
    }
    [Serializable]
    public class NliItem10
    {
        /// <summary>
        /// 
        /// </summary>
        public Desc_obj10 desc_obj;
        /// <summary>
        /// 
        /// </summary>
        public List<Data_objItem10> data_obj;
        /// <summary>
        /// 
        /// </summary>
        public string type;
    }
    [Serializable]
    public class Data10
    {
        /// <summary>
        /// 
        /// </summary>
        public List<NliItem10> nli;
    }
    [Serializable]
    public class Root10
    {
        /// <summary>
        /// 
        /// </summary>
        public string status;
        /// <summary>
        /// 
        /// </summary>
        public Data10 data;
    }


    #endregion


    #region 菜肴选择模块 cooking selection 蛋糕怎么做

    [Serializable]
    public class Desc_obj11
    {
        /// <summary>
        /// 主人，为你精选了一些。你可以说：第几个，或者直接点击查看详情。
        /// </summary>
        public string result;
        /// <summary>
        /// 
        /// </summary>
        public string type;
        /// <summary>
        /// 
        /// </summary>
        public int status;
    }
    [Serializable]
    public class Data_objItem11
    {
        /// <summary>
        /// 烤蛋糕
        /// </summary>
        public string name;
    }
    [Serializable]
    public class NliItem11
    {
        /// <summary>
        /// 
        /// </summary>
        public Desc_obj11 desc_obj;
        /// <summary>
        /// 
        /// </summary>
        public List<Data_objItem11> data_obj;
        /// <summary>
        /// 
        /// </summary>
        public string type;
    }
    [Serializable]
    public class Data11
    {
        /// <summary>
        /// 
        /// </summary>
        public List<NliItem11> nli;
    }
    [Serializable]
    public class Root11
    {
        /// <summary>
        /// 
        /// </summary>
        public string status;
        /// <summary>
        /// 
        /// </summary>
        public Data11 data;
    }


    #endregion


    #region 酒店选择模块  hotel  预定酒店
    [Serializable]
    public class Desc_obj12
    {
        /// <summary>
        /// 主人，有以下酒店可以预订。你可以说：第几个，或者直接点击查看详情。
        /// </summary>
        public string result;
        /// <summary>
        /// 
        /// </summary>
        public string type;
        /// <summary>
        /// 
        /// </summary>
        public int status;
    }
    [Serializable]
    public class Data_objItem12
    {
        /// <summary>
        /// 酒店形象
        /// </summary>
        public string hotel_image;
        /// <summary>
        ///  平均价格
        /// </summary>
        public string max_price;
        /// <summary>
        /// 酒店网站
        /// </summary>
        public string description_url;
        /// <summary>
        /// 携程评级
        /// </summary>
        public string ctrip_rating;
        /// <summary>
        /// 酒店描述
        /// </summary>
        public string description;
        /// <summary>
        /// 酒店地址
        /// </summary>
        public string hotel_address;
        /// <summary>
        /// 最低价格
        /// </summary>
        public string floor_price;
        /// <summary>
        /// 酒店名字
        /// </summary>
        public string hotel_name;
        /// <summary>
        /// 用户评级
        /// </summary>
        public string user_rating;
    }
    [Serializable]
    public class NliItem12
    {
        /// <summary>
        /// 
        /// </summary>
        public Desc_obj12 desc_obj;
        /// <summary>
        /// 
        /// </summary>
        public List<Data_objItem12> data_obj;
        /// <summary>
        /// 
        /// </summary>
        public string type;
    }
    [Serializable]
    public class Data12
    {
        /// <summary>
        /// 
        /// </summary>
        public List<NliItem12> nli;
    }
    [Serializable]
    public class Root12
    {
        /// <summary>
        /// 
        /// </summary>
        public string status;
        /// <summary>
        /// 
        /// </summary>
        public Data12 data;
    }

    #endregion


    #region 诗集模块  poem  我要听长恨歌
    [Serializable]
    public class Desc_obj13
    {
        /// <summary>
        /// 明月几时有，把酒问青天。不知天上宫阙，今夕是何年？我欲乘风归去，又恐琼楼玉宇，高处不胜寒。起舞弄清影，何似在人间！转朱阁，低绮户，照无眠。不应有恨，何事长向别时圆？人有悲欢离合，月有阴晴圆缺，此事古难全。但愿人长久，千里共婵娟。
        /// </summary>
        public string result;
        /// <summary>
        /// 
        /// </summary>
        public int status;
    }
    [Serializable]
    public class Data_objItem13
    {
        /// <summary>
        /// 苏轼
        /// </summary>
        public string author;
        /// <summary>
        /// 水调歌头
        /// </summary>
        public string title;
        /// <summary>
        /// 明月几时有，把酒问青天。不知天上宫阙，今夕是何年？我欲乘风归去，又恐琼楼玉宇，高处不胜寒。起舞弄清影，何似在人间！转朱阁，低绮户，照无眠。不应有恨，何事长向别时圆？人有悲欢离合，月有阴晴圆缺，此事古难全。但愿人长久，千里共婵娟。
        /// </summary>
        public string content;
    }
    [Serializable]
    public class NliItem13
    {
        /// <summary>
        /// 
        /// </summary>
        public Desc_obj13 desc_obj;
        /// <summary>
        /// 
        /// </summary>
        public List<Data_objItem13> data_obj;
        /// <summary>
        /// 
        /// </summary>
        public string type;
    }
    [Serializable]
    public class Data13
    {
        /// <summary>
        /// 
        /// </summary>
        public List<NliItem13> nli;
    }
    [Serializable]
    public class Root13
    {
        /// <summary>
        /// 
        /// </summary>
        public string status;
        /// <summary>
        /// 
        /// </summary>
        public Data13 data;
    }
    #endregion


    //#region 欧拉密返回Json数据
    //{"status":"ok","data":{"nli":[{"desc_obj":{"result":"打开空调就好了。","status":0},"type":"nonsense"}]}}
    //#endregion

}
