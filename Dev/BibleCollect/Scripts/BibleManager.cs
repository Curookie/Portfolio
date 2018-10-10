using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class BibleManager : MonoBehaviour
{

    public static BibleManager _bm = null;


    private DataManager dm;
    private GameManager gm;
    private LevelManager lm;
    private BoxManager boxm;
    private AudioManager am;
    private StageManager sm;

    //Counting Variables
    public static readonly int _verseTotCount = 31101;

    //Perfection
    public Slider _pfSlider;

    public GameObject _G_CardsUI;
    public GameObject _G_AbiMenuBtnsUI;
    public GameObject _G_CardsPerChtUI;
    public GameObject[] _G_CardsPerAbiUI;
    public GameObject _G_VerseCardUI;
    public Texture[] _abilityImages;
    private GameObject _Inst_Verse;
    
    public static int _nowOpenAbility = 1;

    private Color[] VerseColor = new Color[] {
        new Color32(0x00, 0x00, 0x00, 0xFF),
        new Color32(0x75, 0x4C, 0x24, 0xFF),
        new Color32(0x90, 0x91, 0x93, 0xFF),
        new Color32(0xE5, 0xC3, 0x00, 0xFF),
        new Color32(0x49, 0xC4, 0x69, 0xFF),
        new Color32(0x1F, 0xC4, 0xE8, 0xFF),
    };

    private readonly int[] LastVerseCodePerTestamentCode = new int[] { -1/*for Code*/, 1532/*창*/, 2745/*출*/, 3604/*레*/, 4892, 5851, 6509, 7127, 7212, 8021, 8716, 9532, 10251, 11193, 12015, 12295, 12701, 12868, 13938, 16398, 17313, 17535, 17653, 18945, 20309, 20463, 21736, 22093, 22290, 22363, 22509, 22530, 22578, 22683, 22730, 22786, 22839, 22877, 23088, 23143, 24214, 24892, 26043, 26922, 27929, 28362, 28799, 29055, 29204, 29359, 29463, 29558, 29647, 29694, 29807, 29890, 29936, 29961, 30264, 30372, 30477, 30538, 30643, 30656, 30671, 30696, 31100 };
    private readonly int[] AbilityLastTestamentCodePerTestamentCode = new int[] { 0/*for Code*/, /*사랑이*/5, /*2번째*/17, 22, 27, 39, 43, 44, 65, 66 };
    private readonly int[] LastVerseCountPerChapterCode = new int[] { -1/*for Code*/ , 30, 55, 79, 105, 137, 159, 183, 205, 234, 266, 298, 318, 336, 360, 381, 397, 424, 457, 495,
        513, 547, 571, 591, 658, 692, 727, 773, 795, 830, 873, 928, 960, 980, 1011, 1040, 1083, 1119, 1149, 1172, 1195, 1252, 1290, 1324, 1358, 1386, 1420, 1451, 1473, 1506, 1532, 1554, 1579, 1601, 1632, 1655, 1685, 1710, 1742, 1777, 1806, 1816, 1867, 1889, 1920, 1947, 1983, 1999, 2026, 2051, 2077, 2113, 2144, 2177, 2195, 2235, 2272, 2293, 2336, 2382,
        2420, 2438, 2473, 2496, 2531, 2566, 2604, 2633, 2664, 2707, 2745, 2762, 2778, 2795, 2830, 2849, 2879, 2917, 2953, 2977, 2997, 3044, 3052, 3111, 3168, 3201, 3235, 3251, 3281, 3318, 3345, 3369, 3402, 3446, 3469, 3524, 3570, 3604, 3658, 3692, 3743, 3792, 3823, 3850, 3939, 3965, 3988, 4024, 4059, 4075, 4108, 4153, 4194, 4244, 4257, 4289, 4311, 4340,
        4375, 4416, 4446, 4471, 4489, 4554, 4577, 4608, 4648, 4664, 4718, 4760, 4816, 4845, 4879, 4892, 4938, 4975, 5004, 5053, 5086, 5111, 5137, 5157, 5186, 5208, 5240, 5272, 5290, 5319, 5342, 5364, 5384, 5406, 5427, 5447, 5470, 5500, 5525, 5547, 5566, 5585, 5611, 5679, 5708, 5728, 5758, 5810, 5839, 5851, 5869, 5893, 5910, 5934, 5949, 5976, 6002, 6037,
        6064, 6107, 6130, 6154, 6187, 6202, 6265, 6275, 6293, 6321, 6372, 6381, 6426, 6460, 6476, 6509, 6545, 6568, 6599, 6623, 6654, 6694, 6719, 6754, 6811, 6829, 6869, 6884, 6909, 6929, 6949, 6980, 6993, 7024, 7054, 7102, 7127, 7149, 7172, 7190, 7212, 7240, 7276, 7297, 7319, 7331, 7352, 7369, 7391, 7418, 7445, 7460, 7485, 7508, 7560, 7595, 7618, 7676,
        7706, 7730, 7772, 7787, 7810, 7839, 7861, 7905, 7930, 7942, 7967, 7978, 8008, 8021, 8048, 8080, 8119, 8131, 8156, 8179, 8208, 8226, 8239, 8258, 8285, 8316, 8355, 8388, 8425, 8448, 8477, 8510, 8553, 8579, 8601, 8652, 8691, 8716, 8769, 8815, 8843, 8877, 8895, 8933, 8984, 9050, 9078, 9107, 9150, 9183, 9217, 9248, 9282, 9316, 9340, 9386, 9407, 9450,
        9479, 9532, 9550, 9575, 9602, 9646, 9673, 9706, 9726, 9755, 9792, 9828, 9849, 9870, 9895, 9924, 9962, 9982, 10023, 10060, 10097, 10118, 10144, 10164, 10201, 10221, 10251, 10305, 10360, 10384, 10427, 10453, 10534, 10574, 10614, 10658, 10672, 10719, 10759, 10773, 10790, 10819, 10862, 10889, 10906, 10925, 10933, 10963, 10982, 11014, 11045, 11076,
        11108, 11142, 11163, 11193, 11210, 11228, 11245, 11267, 11281, 11323, 11345, 11363, 11394, 11413, 11436, 11452, 11474, 11489, 11508, 11522, 11541, 11575, 11586, 11623, 11643, 11655, 11676, 11703, 11731, 11754, 11763, 11790, 11826, 11853, 11874, 11907, 11932, 11965, 11992, 12015, 12026, 12096, 12109, 12133, 12150, 12172, 12200, 12236, 12251, 12295,
        12306, 12326, 12358, 12381, 12400, 12419, 12492, 12510, 12548, 12587, 12623, 12670, 12701, 12723, 12746, 12761, 12778, 12792, 12806, 12816, 12833, 12865, 12868, 12890, 12903, 12929, 12950, 12977, 13007, 13028, 13050, 13085, 13107, 13127, 13152, 13180, 13202, 13237, 13259, 13275, 13296, 13325, 13354, 13388, 13418, 13435, 13460, 13466, 13480, 13503,
        13531, 13556, 13587, 13627, 13649, 13682, 13719, 13735, 13768, 13792, 13833, 13863, 13887, 13921, 13938, 13944, 13956, 13964, 13972, 13984, 13994, 14011, 14020, 14040, 14058, 14065, 14073, 14079, 14086, 14091, 14102, 14117, 14167, 14181, 14190, 14203, 14234, 14240, 14250, 14272, 14284, 14298, 14307, 14318, 14330, 14354, 14365, 14387, 14409, 14437,
        14449, 14489, 14511, 14524, 14541, 14554, 14565, 14570, 14596, 14613, 14624, 14633, 14647, 14667, 14690, 14709, 14718, 14724, 14731, 14754, 14767, 14778, 14789, 14806, 14818, 14826, 14838, 14849, 14859, 14872, 14892, 14899, 14934, 14970, 14975, 14999, 15018, 15046, 15069, 15079, 15091, 15111, 15183, 15196, 15215, 15231, 15239, 15257, 15269, 15282,
        15299, 15306, 15324, 15376, 15393, 15409, 15424, 15429, 15452, 15463, 15476, 15488, 15497, 15506, 15511, 15519, 15547, 15569, 15604, 15649, 15697, 15740, 15753, 15784, 15791, 15801, 15811, 15820, 15828, 15846, 15865, 15867, 15896, 16072, 16079, 16087, 16096, 16100, 16108, 16113, 16119, 16124, 16130, 16138, 16146, 16149, 16167, 16170, 16173, 16194,
        16220, 16229, 16237, 16261, 16274, 16284, 16291, 16303, 16318, 16339, 16349, 16369, 16383, 16392, 16398, 16431, 16453, 16488, 16515, 16538, 16573, 16600, 16636, 16654, 16686, 16717, 16745, 16770, 16805, 16838, 16871, 16899, 16923, 16952, 16982, 17013, 17042, 17077, 17111, 17139, 17167, 17194, 17222, 17249, 17282, 17313, 17331, 17357, 17379, 17395,
        17415, 17427, 17456, 17473, 17491, 17511, 17521, 17535, 17552, 17569, 17580, 17596, 17612, 17626, 17639, 17653, 17684, 17706, 17732, 17738, 17768, 17781, 17806, 17828, 17849, 17883, 17899, 17905, 17927, 17959, 17968, 17982, 17996, 18003, 18028, 18034, 18051, 18076, 18094, 18117, 18129, 18150, 18163, 18192, 18216, 18249, 18258, 18278, 18302, 18319,
        18329, 18351, 18389, 18411, 18419, 18450, 18479, 18504, 18532, 18560, 18585, 18598, 18613, 18635, 18661, 18672, 18695, 18710, 18722, 18739, 18752, 18764, 18785, 18799, 18820, 18842, 18853, 18865, 18884, 18896, 18921, 18945, 18964, 19001, 19026, 19057, 19088, 19118, 19152, 19174, 19200, 19225, 19248, 19265, 19292, 19314, 19335, 19356, 19383, 19406,
        19421, 19439, 19453, 19483, 19523, 19533, 19571, 19595, 19617, 19634, 19666, 19690, 19730, 19774, 19800, 19822, 19841, 19873, 19894, 19922, 19940, 19956, 19974, 19996, 20009, 20039, 20044, 20072, 20079, 20126, 20165, 20211, 20275, 20309, 20331, 20353, 20419, 20441, 20463, 20491, 20501, 20528, 20545, 20562, 20576, 20603, 20621, 20632, 20654, 20679,
        20707, 20730, 20753, 20761, 20824, 20848, 20880, 20894, 20943, 20975, 21006, 21055, 21082, 21099, 21120, 21156, 21182, 21203, 21229, 21247, 21279, 21312, 21343, 21358, 21396, 21424, 21447, 21476, 21525, 21551, 21571, 21598, 21629, 21654, 21678, 21701, 21736, 21757, 21806, 21836, 21873, 21904, 21932, 21960, 21987, 22014, 22035, 22080, 22093, 22104,
        22127, 22132, 22151, 22166, 22177, 22193, 22207, 22224, 22239, 22251, 22265, 22281, 22290, 22310, 22342, 22363, 22378, 22394, 22409, 22422, 22449, 22463, 22480, 22494, 22509, 22530, 22547, 22557, 22567, 22578, 22594, 22607, 22619, 22632, 22647, 22663, 22683, 22698, 22711, 22730, 22747, 22767, 22786, 22804, 22819, 22839, 22854, 22877, 22898, 22911,
        22921, 22935, 22946, 22961, 22975, 22998, 23015, 23027, 23044, 23058, 23067, 23088, 23102, 23119, 23137, 23143, 23168, 23191, 23208, 23233, 23281, 23315, 23344, 23378, 23416, 23458, 23488, 23538, 23596, 23632, 23671, 23699, 23726, 23761, 23791, 23825, 23871, 23917, 23956, 24007, 24053, 24128, 24194, 24214, 24259, 24287, 24322, 24363, 24406, 24462,
        24499, 24537, 24587, 24639, 24672, 24716, 24753, 24825, 24872, 24892, 24972, 25024, 25062, 25106, 25145, 25194, 25244, 25300, 25362, 25404, 25458, 25517, 25552, 25587, 25619, 25650, 25687, 25730, 25778, 25825, 25863, 25934, 25990, 26043, 26094, 26119, 26155, 26209, 26256, 26327, 26380, 26439, 26480, 26522, 26579, 26629, 26667, 26698, 26725, 26758,
        26784, 26824, 26866, 26897, 26922, 26948, 26995, 27021, 27058, 27100, 27115, 27175, 27215, 27258, 27306, 27336, 27361, 27413, 27441, 27482, 27522, 27556, 27584, 27625, 27663, 27703, 27733, 27768, 27795, 27822, 27854, 27898, 27929, 27961, 27990, 28021, 28046, 28067, 28090, 28115, 28154, 28187, 28208, 28244, 28265, 28279, 28302, 28335, 28362, 28393,
        28409, 28432, 28453, 28466, 28486, 28526, 28539, 28566, 28599, 28633, 28664, 28677, 28717, 28775, 28799, 28823, 28840, 28858, 28876, 28897, 28915, 28931, 28955, 28970, 28988, 29021, 29042, 29055, 29079, 29100, 29129, 29160, 29186, 29204, 29227, 29249, 29270, 29302, 29335, 29359, 29389, 29419, 29440, 29463, 29492, 29515, 29540, 29558, 29568, 29588,
        29601, 29619, 29647, 29659, 29676, 29694, 29714, 29729, 29745, 29761, 29786, 29807, 29825, 29851, 29868, 29890, 29906, 29921, 29936, 29961, 29975, 29993, 30012, 30028, 30042, 30062, 30090, 30103, 30131, 30170, 30210, 30239, 30264, 30291, 30317, 30335, 30352, 30372, 30397, 30422, 30444, 30463, 30477, 30498, 30520, 30538, 30548, 30577, 30601, 30622,
        30643, 30656, 30671, 30696, 30716, 30745, 30767, 30778, 30792, 30809, 30826, 30839, 30860, 30871, 30890, 30907, 30925, 30945, 30953, 30974, 30992, 31016, 31037, 31052, 31079, 31100 };
    private readonly int[] ChapterCodeNumPerTestamentCode = new int[] { 0/*for Code*/, 50, 40, 27, 36, 34, 24, 21, 4, 31, 24, 22, 25, 29, 36, 10, 13, 10, 42, 150, 31, 12, 8, 66, 52, 5, 48, 12, 14, 3, 9, 1, 4, 7, 3, 3, 3, 2, 14, 4, 28, 16, 24, 21, 28, 16, 16, 13, 6, 6, 4, 4, 5, 3, 6, 4, 3, 1, 13, 5, 5, 3, 5, 1, 1, 1, 22 };
    private readonly string[] TestamentNamePerTestamentCode = new string[] {""/*for Code*/, "창세기", "출애굽기", "레위기", "민수기", "신명기", "여호수아", "사사기", "룻기", "사무엘상", "사무엘하", "열왕기상", "열왕기하", "역대상", "역대하", "에스라", "느헤미야", "에스더", "욥기", "시편", "잠언", "전도서", "아가", "이사야", "예레미야", "예레미야 애가", "에스겔", "다니엘", "호세아", "요엘", "아모스", "오바댜", "요나", "미가", "나훔", "하박국", "스바냐", "학개", "스가랴", "말라기",
        "마태복음", "마가복음", "누가복음", "요한복음", "사도행전", "로마서", "고린도전서", "고린도후서", "갈라디아서", "에베소서", "빌립보서", "골로새서", "데살로니가전서", "데살로니가후서", "디모데전서", "디모데후서", "디도서", "빌레몬서", "히브리서", "야고보서", "베드로전서", "베드로후서", "요한1서", "요한2서", "요한3서", "유다서", "요한계시록" };


    private void Awake()
    {
        if (_bm == null)
        {
            _bm = this;
        }
        
        //SetVerseToCht(new BibleCard(43, 2, 1, 1, 300L, 2));
        //ChangeChapterAndVerseUI(new BibleCard(43, 2, 1, 1, 300L, 2));
    }

    private void OnEnable()
    {
        dm = DataManager._dm;
        gm = GameManager._gm;
        am = AudioManager._am;
        lm = LevelManager._lm;
    }

    private void Start()
    {
        InitChapterShowUI();

        CheckPerfection();
    }

    public void CheckPerfection()
    {
        _pfSlider.value = dm.GetOnlyVerseCount();
        _pfSlider.transform.Find("Background/Info").GetComponent<Text>().text = dm.GetOnlyVerseCount() + " / " + _verseTotCount;
        _pfSlider.transform.Find("Percent").GetComponent<Text>().text = string.Format("{0:0.00}", double.Parse(dm.GetOnlyVerseCount() + "") / _verseTotCount * 100) + "%";
    }

    public void InitChapterShowUI()
    {
        for (int i = 1; i < 1190; i++)
        {
            int tC = GetTestamentCodeByChapterCode(i);
            int indexCC = GetIndexOfTestamentPerChapterCode(i, tC);
            //Debug.Log("Mask/G_ab" + GetAbilityCode(tC) + "/G_t" + tC + "/Contents/ChapterBtn (" + indexCC + ")");
            GameObject t = _G_CardsUI.transform.Find("Mask/G_ab" + GetAbilityCode(tC) + "/G_t" + tC + "/Contents/ChapterBtn (" + indexCC + ")").gameObject;
            Slider s = t.transform.Find("Slider").GetComponent<Slider>();
            t.transform.Find("Text").GetComponent<Text>().text = indexCC + "장";
            s.maxValue = GetTotalVerseNum(i);
            t.name = i + "";
        }
    }


    //for Calculate BibleData Functions
    public int GetTotalChapterNum(int TestamentCode)
    {
        /* Used VerseCode
        int prevTmstotNum = 0;
        for (int i = 0; i < 1190; i++)
        {
            if (LastVerseCountPerChapterCode[i] == LastVerseCodePerTestamentCode[TestamentCode - 1])
            {
                prevTmstotNum = i;
            }
            else if (LastVerseCountPerChapterCode[i] == LastVerseCodePerTestamentCode[TestamentCode])
            {
                return i - prevTmstotNum;
            }
        }
        return -1;
        */
        //More Efficient Code
        return ChapterCodeNumPerTestamentCode[TestamentCode];
    }

    public int GetIndexOfTestamentPerChapterCode(int ChapterCode, int TestamentCode)
    {
        for (int i = 0; i < 1190; i++)
        {
            if (LastVerseCountPerChapterCode[i] == LastVerseCodePerTestamentCode[TestamentCode - 1])
            {
                //Debug.Log("i: " + i + ",ChapterCodePerLastVerseCount[i]: " + ChapterCodePerLastVerseCount[i] + ", " + LastVerseCodePerTestamentCode[TestamentCode - 1]);
                return ChapterCode - i;
            }
        }
        return -1;
    }

    public int GetChapterCode(int VerseCode)
    {
        int cC = 0;
        for (int i = 0; i < 1190; i++)
        {
            if (VerseCode <= LastVerseCountPerChapterCode[i])
            {
                cC = i;
                break;
            }
        }
        return cC;
    }

    public string GetTestamentName(int TestamentCode)
    {
        return TestamentNamePerTestamentCode[TestamentCode];
    }

    public long GetAbBibleEnergy(int AbilityCode)
    {
        return dm.GetBibleEnergyPerAbility(AbilityCode);
    }

    public int GetTestamentCode(int VerseCode)
    {
        int tC = 0;
        for (int i = 0; i < 67; i++)
        {
            if (VerseCode <= LastVerseCodePerTestamentCode[i])
            {
                tC = i;
                break;
            }
        }
        return tC;
    }

    public int GetTestamentCodeByChapterCode(int ChapterCode)
    {
        int tC = 0;
        for (int i = 0; i < 67; i++)
        {
            if (LastVerseCountPerChapterCode[ChapterCode] <= LastVerseCodePerTestamentCode[i])
            {
                tC = i;
                break;
            }
        }
        return tC;
    }

    public int GetTotalVerseNumPerTestament(int TestamentCode)
    {
        return LastVerseCodePerTestamentCode[TestamentCode] - LastVerseCodePerTestamentCode[TestamentCode - 1];
    }

    public int GetAbilityCode(int TestamentCode)
    {
        int aC = 0;
        for (int i = 0; i < 10; i++)
        {
            if (TestamentCode <= AbilityLastTestamentCodePerTestamentCode[i])
            {
                aC = i;
                break;
            }
        }
        return aC;
    }

    public List<int> GetTestamentsCodePerAbilityCode(int AbilityCode)
    {
        List<int> tmtCodes = new List<int>();
        for (int j = AbilityLastTestamentCodePerTestamentCode[AbilityCode-1] + 1; j <= AbilityLastTestamentCodePerTestamentCode[AbilityCode]; j++)
        {
            tmtCodes.Add(j);
        }
        return tmtCodes;
    }

    public int GetFirstTestamentCodePerAbilityCode(int AbilityCode)
    {
        return AbilityLastTestamentCodePerTestamentCode[AbilityCode - 1] + 1;
    }

    public int GetFirstChapterCodePerTestamentCode(int TestamentCode)
    {
        if (TestamentCode == 1) return 1;
        for (int i=0; i< 1190; i++)
        {
            if (LastVerseCountPerChapterCode[i] == LastVerseCodePerTestamentCode[TestamentCode-1])
                return i + 1;
        }
        return -1;
    }

    public int GetFirstVerseCodePerChapterCode(int ChapterCode)
    {
        return LastVerseCountPerChapterCode[ChapterCode - 1] + 1;
    }

    public int GetLastVerseCodePerChapterCode(int ChapterCode)
    {
        return LastVerseCountPerChapterCode[ChapterCode];
    }

    public int GetFirstVerseCodePerTestamentCode(int TestamentCode)
    {
        return LastVerseCodePerTestamentCode[TestamentCode - 1] + 1;
    }

    public int GetLastVerseCodePerTestamentCode(int TestamentCode)
    {
        return LastVerseCodePerTestamentCode[TestamentCode];
    }

    public int GetFirstVerseCodePerAbilityCode(int AbilityCode)
    {
        return LastVerseCodePerTestamentCode[AbilityLastTestamentCodePerTestamentCode[AbilityCode - 1]] + 1;
    }

    public int GetLastVerseCodePerAbilityCode(int AbilityCode)
    {
        return LastVerseCodePerTestamentCode[AbilityLastTestamentCodePerTestamentCode[AbilityCode]];
    }

    public int GetTotalVerseNumPerAbility(int AbilityCode)
    {
        int t = 0;
        for (int i = AbilityLastTestamentCodePerTestamentCode[AbilityCode - 1] + 1; i <= AbilityLastTestamentCodePerTestamentCode[AbilityCode]; i++)
        {
            if (i <= 0) continue;
            t += GetTotalVerseNumPerTestament(i);
        }
        return t;
    }

    public int GetHasOnlyVerseNumPerAbility(int AbilityCode)
    {
        return dm.GetOnlyVerseCountPerAbility(AbilityCode);
    }

    public int GetTotalVerseNum(int ChapterCode)
    {
        return LastVerseCountPerChapterCode[ChapterCode] - LastVerseCountPerChapterCode[ChapterCode - 1];
    }

    public int GetIndexVerseAtChapterCode(int VerseCode, int ChaterCode)
    {
        return VerseCode - LastVerseCountPerChapterCode[ChaterCode - 1];
    }

    //for check Overlap
    public bool IsVerseDISTINCT(int VerseCode)
    {
        //Debug.Log(!forChkVerse[VerseCode]);
        if (!dm.GetForCheckVerseTable(VerseCode))
            return true;
        else
            return false;
    }

    //Save Verse Card
    public void SetVerseToCht(BibleCard c)
    {
        int cV = c.GetVerseCode();
        if (IsVerseDISTINCT(cV))
        {
            dm.PutOnlyVerseCount(1);
            dm.PutOnlyVerseCountPerAbility(c.GetAbilityCode(),1);
            dm.GetVerseCodeAndBestRank().Add(cV, c.GetCardRareRate());
            dm.SaveOD();
            dm.PutForCheckVerseTable(cV, true);
            ChangeChapterAndVerseUI(c);
            CheckPerfection();
            lm.RefreshAbilityLvUI();
        }
        //Overlap 2 or 3 or .. etc
        else
        {
            int a;
            if (dm.GetVerseCodeAndOverlapCount().TryGetValue(cV, out a))
            {
                if (a >= 5)
                {
                    //refuse full stack show pop-up
                    return;
                }
                dm.GetVerseCodeAndOverlapCount()[cV] += 1;
                dm.SaveOD();
            }
            //if FirstTime Overlap
            else
            {
                dm.GetVerseCodeAndOverlapCount().Add(cV, 2);
                dm.SaveOD();
            }

            //for check Show RareImage if Overlap Situation
            if (c.GetCardRareRate() >= dm.GetVerseCodeAndBestRank()[cV])
            {
                dm.GetVerseCodeAndBestRank()[cV] = c.GetCardRareRate();
                dm.SaveOD();
            }
            ChangeOverlapCardUI(c);
        }
        //common add Verse card info
        dm.PutHasTotalVerseCount(1);
        dm.GetVerseList(c.GetChapterCode()).Add(c);
        dm.SaveCD();

        long bE = c.GetBibleEnergy();

        //Add BE
        gm.AddBibleEnergy(bE);

        dm.PutBibleEnergyPerChapter(c.GetChapterCode(), bE);
        dm.PutBibleEnergyPerAbility(c.GetAbilityCode(), bE);
        dm.SaveVD();
    }


    //if Not Overlap Change UI 
    public void ChangeChapterAndVerseUI(BibleCard c)
    {
        //Debug.Log("Mask/G_ab" + c.GetAbilityCode() + "/G_t" + c.GetTestamentCode() + "/Contents/ChapterBtn (" + GetIndexOfTestamentPerChapterCode(c.GetChapterCode(), c.GetTestamentCode()) + ")");
        GameObject t = _G_CardsUI.transform.Find("Mask/G_ab" + c.GetAbilityCode() + "/G_t" + c.GetTestamentCode() + "/Contents/" + c.GetChapterCode()).gameObject;
        _G_AbiMenuBtnsUI.transform.Find("Abibtn" + c.GetAbilityCode()).GetComponent<Outline>().enabled = true;
        t.GetComponent<Outline>().enabled = true;
        t.transform.Find("Slider").GetComponent<Slider>().value += 1;
    }

    //if Overlap Change UI 
    public void ChangeOverlapCardUI(BibleCard c)
    {
        _G_AbiMenuBtnsUI.transform.Find("Abibtn" + c.GetAbilityCode()).GetComponent<Outline>().enabled = true;
    }

    //not develop for back button
    public void MoveChaptertoVerse(int ChapterCode)
    {
        _G_CardsPerAbiUI[_nowOpenAbility - 1].GetComponent<CanvasGroup>().alpha = 0;
        _G_CardsPerAbiUI[_nowOpenAbility - 1].GetComponent<CanvasGroup>().interactable = false;
        _G_CardsPerAbiUI[_nowOpenAbility - 1].GetComponent<CanvasGroup>().blocksRaycasts = false;
        _G_CardsPerChtUI.SetActive(true);
        ShowingVerseList(ChapterCode);
    }

    //Abi Button Pressed
    public void MoveAbilityTab(int AbilityCode)
    {
        ClearVerseTab();
        _G_CardsPerAbiUI[_nowOpenAbility - 1].GetComponent<CanvasGroup>().alpha = 0;
        _G_CardsPerAbiUI[_nowOpenAbility - 1].GetComponent<CanvasGroup>().interactable = false;
        _G_CardsPerAbiUI[_nowOpenAbility - 1].GetComponent<CanvasGroup>().blocksRaycasts = false;
        _G_AbiMenuBtnsUI.transform.Find("Abibtn" + _nowOpenAbility + "/RawImage").GetComponent<RawImage>().texture = _abilityImages[2 * _nowOpenAbility - 1];
        _nowOpenAbility = AbilityCode;
        _G_CardsPerAbiUI[_nowOpenAbility - 1].GetComponent<CanvasGroup>().alpha = 1;
        _G_CardsPerAbiUI[_nowOpenAbility - 1].GetComponent<CanvasGroup>().interactable = true;
        _G_CardsPerAbiUI[_nowOpenAbility - 1].GetComponent<CanvasGroup>().blocksRaycasts = true;
        _G_CardsPerChtUI.transform.parent.transform.GetComponent<ScrollRect>().content = _G_CardsPerAbiUI[_nowOpenAbility - 1].GetComponent<RectTransform>();
        _G_AbiMenuBtnsUI.transform.Find("Abibtn" + _nowOpenAbility + "/RawImage").GetComponent<RawImage>().texture = _abilityImages[2 * (_nowOpenAbility - 1)];
        _G_AbiMenuBtnsUI.transform.Find("Abibtn" + _nowOpenAbility).GetComponent<Outline>().enabled = false;
        lm.RefreshAbilityLvUI();
    }

    //for VerseTab Closed
    public void ClearVerseTab()
    {
        Destroy(_Inst_Verse);
    }

    //for Insert Enter to Verse's Title ex) 마'/n'20:11
    public int CheckNumberAtString(string s)
    {
        int i = 0;
        Regex numRegex = new Regex(@"[0-9]");
        for (i = 0; i < s.Length; i++)
        {
            if (numRegex.IsMatch(s[i] + ""))
                break;
        }
        return i;
    }

    //for VerseTab ShowUI
    public void ShowingVerseList(int ChapterCode)
    {
        Destroy(_Inst_Verse);
        _Inst_Verse = Instantiate(_G_VerseCardUI, _G_CardsPerChtUI.transform);
        for (int i = 179; i > GetTotalVerseNum(ChapterCode); i--)
        {
            _Inst_Verse.transform.Find("Contents/noneVerse (" + i + ")").gameObject.GetComponent<Image>().enabled = false;
        }
        _Inst_Verse.transform.Find("Chapter").GetComponent<Text>().text = GetTestamentName(GetTestamentCodeByChapterCode(ChapterCode)) + " " + GetIndexOfTestamentPerChapterCode(ChapterCode, GetTestamentCodeByChapterCode(ChapterCode)) + "장";
        foreach (BibleCard a in dm.GetVerseList(ChapterCode))
        {
            int vc = a.GetVerseCode();
            int OverlapRank = 1;
            //Debug.Log(verseminiCard[a.GetCardRareRate() - 1]);
            GameObject g = _Inst_Verse.transform.Find("Contents/noneVerse (" + GetIndexVerseAtChapterCode(vc, a.GetChapterCode()) + ")").gameObject;
            GameObject g2 = g.transform.Find("Text").gameObject;
            string title = DataManager.bd[vc].title;
            int i = CheckNumberAtString(title);
            if (dm.GetVerseCodeAndBestRank().TryGetValue(vc, out OverlapRank))
            {
                //Overlap 수정
                g.GetComponent<Image>().color = VerseColor[OverlapRank];
            }
            else
            {
                g.GetComponent<Image>().color = VerseColor[a.GetCardRareRate()];
            }
            g2.GetComponent<Text>().enabled = true;
            g2.GetComponent<Text>().text = title.Substring(0, i) + "\n" + title.Substring(i);
            //g = verseminiCard[a.GetCardRareRate()-1];

        }
        _G_CardsPerChtUI.transform.parent.transform.GetComponent<ScrollRect>().content = _Inst_Verse.GetComponent<RectTransform>();
    }

    public void BackChapterList(int ChapterCode)
    {

    }


    //For Chapter's Verse Calculator
    void lalalalala()
    {
        string s = "";
        string[] bdSource = new string[31101];
        int count = 0;
        int tot = 0;
        int totChapter = 0;
        int best = 0;
        for (int i = 0; i < _verseTotCount; i++)
        {
            bdSource[i] = DataManager.bd[i].title.Split(':')[0];
            if (i == 0) count++;
            else
            {
                if (i == 31100)
                {
                    count++;
                    tot += count;
                    totChapter += 1;
                    if (best <= count)
                        best = count;
                    s += count + ", ";
                }
                else if (bdSource[i - 1] == bdSource[i])
                {
                    count++;
                }
                else
                {
                    tot += count;
                    s += count + ", ";
                    totChapter += 1;
                    //count++;
                    if (best <= count)
                        best = count;
                    count = 1;
                }

            }
        }
        Debug.Log(s + "tot: " + tot + ",totChapter: " + totChapter + ", best: " + best);

        //Debug.Log(s+"tot: "+count+",totChapter: "+totChapter);
        /* 30, 55, 79, 105, 137, 159, 183, 205, 234, 266, 298, 318, 336, 360, 381, 397, 424, 457, 495, 513, 547, 571,
         * 591, 658, 692, 727, 773, 795, 830, 873, 928, 960, 980, 1011, 1040, 1083, 1119, 1149, 1172, 1195, 1252, 1290,
         * 1324, 1358, 1386, 1420, 1451, 1473, 1506, 1532, 1554, 1579, 1601, 1632, 1655, 1685, 1710, 1742, 1777, 1806,
         * 1816, 1867, 1889, 1920, 1947, 1983, 1999, 2026, 2051, 2077, 2113, 2144, 2177, 2195, 2235, 2272, 2293, 2336,
         * 2382, 2420, 2438, 2473, 2496, 2531, 2566, 2604, 2633, 2664, 2707, 2745, 2762, 2778, 2795, 2830, 2849, 2879,
         * 2917, 2953, 2977, 2997, 3044, 3052, 3111, 3168, 3201, 3235, 3251, 3281, 3318, 3345, 3369, 3402, 3446, 3469,
         * 3524, 3570, 3604, 3658, 3692, 3743, 3792, 3823, 3850, 3939, 3965, 3988, 4024, 4059, 4075, 4108, 4153, 4194,
         * 4244, 4257, 4289, 4311, 4340, 4375, 4416, 4446, 4471, 4489, 4554, 4577, 4608, 4648, 4664, 4718, 4760, 4816,
         * 4845, 4879, 4892, 4938, 4975, 5004, 5053, 5086, 5111, 5137, 5157, 5186, 5208, 5240, 5272, 5290, 5319, 5342,
         * 5364, 5384, 5406, 5427, 5447, 5470, 5500, 5525, 5547, 5566, 5585, 5611, 5679, 5708, 5728, 5758, 5810, 5839,
         * 5851, 5869, 5893, 5910, 5934, 5949, 5976, 6002, 6037, 6064, 6107, 6130, 6154, 6187, 6202, 6265, 6275, 6293,
         * 6321, 6372, 6381, 6426, 6460, 6476, 6509, 6545, 6568, 6599, 6623, 6654, 6694, 6719, 6754, 6811, 6829, 6869,
         * 6884, 6909, 6929, 6949, 6980, 6993, 7024, 7054, 7102, 7127, 7149, 7172, 7190, 7212, 7240, 7276, 7297, 7319,
         * 7331, 7352, 7369, 7391, 7418, 7445, 7460, 7485, 7508, 7560, 7595, 7618, 7676, 7706, 7730, 7772, 7787, 7810,
         * 7839, 7861, 7905, 7930, 7942, 7967, 7978, 8008, 8021, 8048, 8080, 8119, 8131, 8156, 8179, 8208, 8226, 8239,
         * 8258, 8285, 8316, 8355, 8388, 8425, 8448, 8477, 8510, 8553, 8579, 8601, 8652, 8691, 8716, 8769, 8815, 8843,
         * 8877, 8895, 8933, 8984, 9050, 9078, 9107, 9150, 9183, 9217, 9248, 9282, 9316, 9340, 9386, 9407, 9450, 9479,
         * 9532, 9550, 9575, 9602, 9646, 9673, 9706, 9726, 9755, 9792, 9828, 9849, 9870, 9895, 9924, 9962, 9982, 10023,
         * 10060, 10097, 10118, 10144, 10164, 10201, 10221, 10251, 10305, 10360, 10384, 10427, 10453, 10534, 10574,
         * 10614, 10658, 10672, 10719, 10759, 10773, 10790, 10819, 10862, 10889, 10906, 10925, 10933, 10963, 10982,
         * 11014, 11045, 11076, 11108, 11142, 11163, 11193, 11210, 11228, 11245, 11267, 11281, 11323, 11345, 11363,
         * 11394, 11413, 11436, 11452, 11474, 11489, 11508, 11522, 11541, 11575, 11586, 11623, 11643, 11655, 11676,
         * 11703, 11731, 11754, 11763, 11790, 11826, 11853, 11874, 11907, 11932, 11965, 11992, 12015, 12026, 12096,
         * 12109, 12133, 12150, 12172, 12200, 12236, 12251, 12295, 12306, 12326, 12358, 12381, 12400, 12419, 12492,
         * 12510, 12548, 12587, 12623, 12670, 12701, 12723, 12746, 12761, 12778, 12792, 12806, 12816, 12833, 12865,
         * 12868, 12890, 12903, 12929, 12950, 12977, 13007, 13028, 13050, 13085, 13107, 13127, 13152, 13180, 13202,
         * 13237, 13259, 13275, 13296, 13325, 13354, 13388, 13418, 13435, 13460, 13466, 13480, 13503, 13531, 13556,
         * 13587, 13627, 13649, 13682, 13719, 13735, 13768, 13792, 13833, 13863, 13887, 13921, 13938, 13944, 13956,
         * 13964, 13972, 13984, 13994, 14011, 14020, 14040, 14058, 14065, 14073, 14079, 14086, 14091, 14102, 14117,
         * 14167, 14181, 14190, 14203, 14234, 14240, 14250, 14272, 14284, 14298, 14307, 14318, 14330, 14354, 14365,
         * 14387, 14409, 14437, 14449, 14489, 14511, 14524, 14541, 14554, 14565, 14570, 14596, 14613, 14624, 14633,
         * 14647, 14667, 14690, 14709, 14718, 14724, 14731, 14754, 14767, 14778, 14789, 14806, 14818, 14826, 14838,
         * 14849, 14859, 14872, 14892, 14899, 14934, 14970, 14975, 14999, 15018, 15046, 15069, 15079, 15091, 15111,
         * 15183, 15196, 15215, 15231, 15239, 15257, 15269, 15282, 15299, 15306, 15324, 15376, 15393, 15409, 15424,
         * 15429, 15452, 15463, 15476, 15488, 15497, 15506, 15511, 15519, 15547, 15569, 15604, 15649, 15697, 15740,
         * 15753, 15784, 15791, 15801, 15811, 15820, 15828, 15846, 15865, 15867, 15896, 16072, 16079, 16087, 16096,
         * 16100, 16108, 16113, 16119, 16124, 16130, 16138, 16146, 16149, 16167, 16170, 16173, 16194, 16220, 16229,
         * 16237, 16261, 16274, 16284, 16291, 16303, 16318, 16339, 16349, 16369, 16383, 16392, 16398, 16431, 16453,
         * 16488, 16515, 16538, 16573, 16600, 16636, 16654, 16686, 16717, 16745, 16770, 16805, 16838, 16871, 16899, 
         * 16923, 16952, 16982, 17013, 17042, 17077, 17111, 17139, 17167, 17194, 17222, 17249, 17282, 17313, 17331,
         * 17357, 17379, 17395, 17415, 17427, 17456, 17473, 17491, 17511, 17521, 17535, 17552, 17569, 17580, 17596,
         * 17612, 17626, 17639, 17653, 17684, 17706, 17732, 17738, 17768, 17781, 17806, 17828, 17849, 17883, 17899,
         * 17905, 17927, 17959, 17968, 17982, 17996, 18003, 18028, 18034, 18051, 18076, 18094, 18117, 18129, 18150,
         * 18163, 18192, 18216, 18249, 18258, 18278, 18302, 18319, 18329, 18351, 18389, 18411, 18419, 18450, 18479, 
         * 18504, 18532, 18560, 18585, 18598, 18613, 18635, 18661, 18672, 18695, 18710, 18722, 18739, 18752, 18764,
         * 18785, 18799, 18820, 18842, 18853, 18865, 18884, 18896, 18921, 18945, 18964, 19001, 19026, 19057, 19088,
         * 19118, 19152, 19174, 19200, 19225, 19248, 19265, 19292, 19314, 19335, 19356, 19383, 19406, 19421, 19439,
         * 19453, 19483, 19523, 19533, 19571, 19595, 19617, 19634, 19666, 19690, 19730, 19774, 19800, 19822, 19841,
         * 19873, 19894, 19922, 19940, 19956, 19974, 19996, 20009, 20039, 20044, 20072, 20079, 20126, 20165, 20211,
         * 20275, 20309, 20331, 20353, 20419, 20441, 20463, 20491, 20501, 20528, 20545, 20562, 20576, 20603, 20621, 
         * 20632, 20654, 20679, 20707, 20730, 20753, 20761, 20824, 20848, 20880, 20894, 20943, 20975, 21006, 21055,
         * 21082, 21099, 21120, 21156, 21182, 21203, 21229, 21247, 21279, 21312, 21343, 21358, 21396, 21424, 21447,
         * 21476, 21525, 21551, 21571, 21598, 21629, 21654, 21678, 21701, 21736, 21757, 21806, 21836, 21873, 21904,
         * 21932, 21960, 21987, 22014, 22035, 22080, 22093, 22104, 22127, 22132, 22151, 22166, 22177, 22193, 22207,
         * 22224, 22239, 22251, 22265, 22281, 22290, 22310, 22342, 22363, 22378, 22394, 22409, 22422, 22449, 22463, 
         * 22480, 22494, 22509, 22530, 22547, 22557, 22567, 22578, 22594, 22607, 22619, 22632, 22647, 22663, 22683,
         * 22698, 22711, 22730, 22747, 22767, 22786, 22804, 22819, 22839, 22854, 22877, 22898, 22911, 22921, 22935, 
         * 22946, 22961, 22975, 22998, 23015, 23027, 23044, 23058, 23067, 23088, 23102, 23119, 23137, 23143, 23168,
         * 23191, 23208, 23233, 23281, 23315, 23344, 23378, 23416, 23458, 23488, 23538, 23596, 23632, 23671, 23699, 
         * 23726, 23761, 23791, 23825, 23871, 23917, 23956, 24007, 24053, 24128, 24194, 24214, 24259, 24287, 24322,
         * 24363, 24406, 24462, 24499, 24537, 24587, 24639, 24672, 24716, 24753, 24825, 24872, 24892, 24972, 25024, 
         * 25062, 25106, 25145, 25194, 25244, 25300, 25362, 25404, 25458, 25517, 25552, 25587, 25619, 25650, 25687,
         * 25730, 25778, 25825, 25863, 25934, 25990, 26043, 26094, 26119, 26155, 26209, 26256, 26327, 26380, 26439, 
         * 26480, 26522, 26579, 26629, 26667, 26698, 26725, 26758, 26784, 26824, 26866, 26897, 26922, 26948, 26995,
         * 27021, 27058, 27100, 27115, 27175, 27215, 27258, 27306, 27336, 27361, 27413, 27441, 27482, 27522, 27556, 
         * 27584, 27625, 27663, 27703, 27733, 27768, 27795, 27822, 27854, 27898, 27929, 27961, 27990, 28021, 28046, 
         * 28067, 28090, 28115, 28154, 28187, 28208, 28244, 28265, 28279, 28302, 28335, 28362, 28393, 28409, 28432, 
         * 28453, 28466, 28486, 28526, 28539, 28566, 28599, 28633, 28664, 28677, 28717, 28775, 28799, 28823, 28840,
         * 28858, 28876, 28897, 28915, 28931, 28955, 28970, 28988, 29021, 29042, 29055, 29079, 29100, 29129, 29160, 
         * 29186, 29204, 29227, 29249, 29270, 29302, 29335, 29359, 29389, 29419, 29440, 29463, 29492, 29515, 29540, 
         * 29558, 29568, 29588, 29601, 29619, 29647, 29659, 29676, 29694, 29714, 29729, 29745, 29761, 29786, 29807, 
         * 29825, 29851, 29868, 29890, 29906, 29921, 29936, 29961, 29975, 29993, 30012, 30028, 30042, 30062, 30090,
         * 30103, 30131, 30170, 30210, 30239, 30264, 30291, 30317, 30335, 30352, 30372, 30397, 30422, 30444, 30463, 
         * 30477, 30498, 30520, 30538, 30548, 30577, 30601, 30622, 30643, 30656, 30671, 30696, 30716, 30745, 30767,
         * 30778, 30792, 30809, 30826, 30839, 30860, 30871, 30890, 30907, 30925, 30945, 30953, 30974, 30992, 31016, 
         * 31037, 31052, 31079, 31100 */

        //Debug.Log(s+"tot: "+tot+",totChapter: "+totChapter);
        /* 31, 25, 24, 26, 32, 22, 24, 22, 29, 32, 32, 20, 18, 24, 21, 16, 27, 33, 38, 18, 34, 24, 20, 67, 34, 35, 
         * 46, 22, 35, 43, 55, 32, 20, 31, 29, 43, 36, 30, 23, 23, 57, 38, 34, 34, 28, 34, 31, 22, 33, 26, 22, 25, 
         * 22, 31, 23, 30, 25, 32, 35, 29, 10, 51, 22, 31, 27, 36, 16, 27, 25, 26, 36, 31, 33, 18, 40, 37, 21, 43, 
         * 46, 38, 18, 35, 23, 35, 35, 38, 29, 31, 43, 38, 17, 16, 17, 35, 19, 30, 38, 36, 24, 20, 47, 8, 59, 57, 
         * 33, 34, 16, 30, 37, 27, 24, 33, 44, 23, 55, 46, 34, 54, 34, 51, 49, 31, 27, 89, 26, 23, 36, 35, 16, 33, 
         * 45, 41, 50, 13, 32, 22, 29, 35, 41, 30, 25, 18, 65, 23, 31, 40, 16, 54, 42, 56, 29, 34, 13, 46, 37, 29, 
         * 49, 33, 25, 26, 20, 29, 22, 32, 32, 18, 29, 23, 22, 20, 22, 21, 20, 23, 30, 25, 22, 19, 19, 26, 68, 29, 
         * 20, 30, 52, 29, 12, 18, 24, 17, 24, 15, 27, 26, 35, 27, 43, 23, 24, 33, 15, 63, 10, 18, 28, 51, 9, 45, 
         * 34, 16, 33, 36, 23, 31, 24, 31, 40, 25, 35, 57, 18, 40, 15, 25, 20, 20, 31, 13, 31, 30, 48, 25, 22, 23, 
         * 18, 22, 28, 36, 21, 22, 12, 21, 17, 22, 27, 27, 15, 25, 23, 52, 35, 23, 58, 30, 24, 42, 15, 23, 29, 22, 
         * 44, 25, 12, 25, 11, 30, 13, 27, 32, 39, 12, 25, 23, 29, 18, 13, 19, 27, 31, 39, 33, 37, 23, 29, 33, 43, 
         * 26, 22, 51, 39, 25, 53, 46, 28, 34, 18, 38, 51, 66, 28, 29, 43, 33, 34, 31, 34, 34, 24, 46, 21, 43, 29, 
         * 53, 18, 25, 27, 44, 27, 33, 20, 29, 37, 36, 21, 21, 25, 29, 38, 20, 41, 37, 37, 21, 26, 20, 37, 20, 30, 
         * 54, 55, 24, 43, 26, 81, 40, 40, 44, 14, 47, 40, 14, 17, 29, 43, 27, 17, 19, 8, 30, 19, 32, 31, 31, 32, 
         * 34, 21, 30, 17, 18, 17, 22, 14, 42, 22, 18, 31, 19, 23, 16, 22, 15, 19, 14, 19, 34, 11, 37, 20, 12, 21, 
         * 27, 28, 23, 9, 27, 36, 27, 21, 33, 25, 33, 27, 23, 11, 70, 13, 24, 17, 22, 28, 36, 15, 44, 11, 20, 32, 
         * 23, 19, 19, 73, 18, 38, 39, 36, 47, 31, 22, 23, 15, 17, 14, 14, 10, 17, 32, 3, 22, 13, 26, 21, 27, 30, 
         * 21, 22, 35, 22, 20, 25, 28, 22, 35, 22, 16, 21, 29, 29, 34, 30, 17, 25, 6, 14, 23, 28, 25, 31, 40, 22, 
         * 33, 37, 16, 33, 24, 41, 30, 24, 34, 17, 6, 12, 8, 8, 12, 10, 17, 9, 20, 18, 7, 8, 6, 7, 5, 11, 15, 50, 
         * 14, 9, 13, 31, 6, 10, 22, 12, 14, 9, 11, 12, 24, 11, 22, 22, 28, 12, 40, 22, 13, 17, 13, 11, 5, 26, 17, 
         * 11, 9, 14, 20, 23, 19, 9, 6, 7, 23, 13, 11, 11, 17, 12, 8, 12, 11, 10, 13, 20, 7, 35, 36, 5, 24, 19, 28,
         * 23, 10, 12, 20, 72, 13, 19, 16, 8, 18, 12, 13, 17, 7, 18, 52, 17, 16, 15, 5, 23, 11, 13, 12, 9, 9, 5, 8,
         * 28, 22, 35, 45, 48, 43, 13, 31, 7, 10, 10, 9, 8, 18, 19, 2, 29, 176, 7, 8, 9, 4, 8, 5, 6, 5, 6, 8, 8, 3,
         * 18, 3, 3, 21, 26, 9, 8, 24, 13, 10, 7, 12, 15, 21, 10, 20, 14, 9, 6, 33, 22, 35, 27, 23, 35, 27, 36, 18,
         * 32, 31, 28, 25, 35, 33, 33, 28, 24, 29, 30, 31, 29, 35, 34, 28, 28, 27, 28, 27, 33, 31, 18, 26, 22, 16,
         * 20, 12, 29, 17, 18, 20, 10, 14, 17, 17, 11, 16, 16, 14, 13, 14, 31, 22, 26, 6, 30, 13, 25, 22, 21, 34, 16,
         * 6, 22, 32, 9, 14, 14, 7, 25, 6, 17, 25, 18, 23, 12, 21, 13, 29, 24, 33, 9, 20, 24, 17, 10, 22, 38, 22, 8,
         * 31, 29, 25, 28, 28, 25, 13, 15, 22, 26, 11, 23, 15, 12, 17, 13, 12, 21, 14, 21, 22, 11, 12, 19, 12, 25, 24,
         * 19, 37, 25, 31, 31, 30, 34, 22, 26, 25, 23, 17, 27, 22, 21, 21, 27, 23, 15, 18, 14, 30, 40, 10, 38, 24, 22,
         * 17, 32, 24, 40, 44, 26, 22, 19, 32, 21, 28, 18, 16, 18, 22, 13, 30, 5, 28, 7, 47, 39, 46, 64, 34, 22, 22,
         * 66, 22, 22, 28, 10, 27, 17, 17, 14, 27, 18, 11, 22, 25, 28, 23, 23, 8, 63, 24, 32, 14, 49, 32, 31, 49, 27,
         * 17, 21, 36, 26, 21, 26, 18, 32, 33, 31, 15, 38, 28, 23, 29, 49, 26, 20, 27, 31, 25, 24, 23, 35, 21, 49, 30,
         * 37, 31, 28, 28, 27, 27, 21, 45, 13, 11, 23, 5, 19, 15, 11, 16, 14, 17, 15, 12, 14, 16, 9, 20, 32, 21, 15,
         * 16, 15, 13, 27, 14, 17, 14, 15, 21, 17, 10, 10, 11, 16, 13, 12, 13, 15, 16, 20, 15, 13, 19, 17, 20, 19, 18,
         * 15, 20, 15, 23, 21, 13, 10, 14, 11, 15, 14, 23, 17, 12, 17, 14, 9, 21, 14, 17, 18, 6, 25, 23, 17, 25, 48,
         * 34, 29, 34, 38, 42, 30, 50, 58, 36, 39, 28, 27, 35, 30, 34, 46, 46, 39, 51, 46, 75, 66, 20, 45, 28, 35, 41,
         * 43, 56, 37, 38, 50, 52, 33, 44, 37, 72, 47, 20, 80, 52, 38, 44, 39, 49, 50, 56, 62, 42, 54, 59, 35, 35, 32,
         * 31, 37, 43, 48, 47, 38, 71, 56, 53, 51, 25, 36, 54, 47, 71, 53, 59, 41, 42, 57, 50, 38, 31, 27, 33, 26, 40,
         * 42, 31, 25, 26, 47, 26, 37, 42, 15, 60, 40, 43, 48, 30, 25, 52, 28, 41, 40, 34, 28, 41, 38, 40, 30, 35, 27,
         * 27, 32, 44, 31, 32, 29, 31, 25, 21, 23, 25, 39, 33, 21, 36, 21, 14, 23, 33, 27, 31, 16, 23, 21, 13, 20, 40,
         * 13, 27, 33, 34, 31, 13, 40, 58, 24, 24, 17, 18, 18, 21, 18, 16, 24, 15, 18, 33, 21, 13, 24, 21, 29, 31, 26,
         * 18, 23, 22, 21, 32, 33, 24, 30, 30, 21, 23, 29, 23, 25, 18, 10, 20, 13, 18, 28, 12, 17, 18, 20, 15, 16, 16,
         * 25, 21, 18, 26, 17, 22, 16, 15, 15, 25, 14, 18, 19, 16, 14, 20, 28, 13, 28, 39, 40, 29, 25, 27, 26, 18, 17,
         * 20, 25, 25, 22, 19, 14, 21, 22, 18, 10, 29, 24, 21, 21, 13, 15, 25, 20, 29, 22, 11, 14, 17, 17, 13, 21, 11,
         * 19, 17, 18, 20, 8, 21, 18, 24, 21, 15, 27, 21*/
    }

    //For Testament's Chapter Calculator
    void lalalalala2()
    {
        string s = "";
        int totChapter = 0;
        for (int i = 1; i < 67; i++)
        {
            s += GetTotalChapterNum(i) + ", ";
            totChapter += GetTotalChapterNum(i);
        }
        Debug.Log(s + "totChapter: " + totChapter);
    }
}
