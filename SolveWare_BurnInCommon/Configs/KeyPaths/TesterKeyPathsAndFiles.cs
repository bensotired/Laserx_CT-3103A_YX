namespace SolveWare_BurnInCommon
{
    public static class TesterKeyPathsAndFiles
    {

        public static string TEST_EXECUTOR_FILES_PATH { get; set; } = "TEST_EXECUTOR_FILES_PATH";
        public static string TEST_EXECUTIVE_COMBO_PATH { get; set; } = "TEST_EXECUTIVE_COMBO_PATH";
        public static string TEST_RECIPE_PATH { get; set; } = "TEST_RECIPE_PATH";
        public static string CALC_RECIPE_PATH { get; set; } = "CALC_RECIPE_PATH";
        public static string TEST_PROFILE_PATH { get; set; } = "TEST_PROFILE_PATH";

        public static string TEST_STREAM_DATA_PATH { get; set; } = "TEST_STREAM_DATA_PATH";
        public static string DATA_BASE_CONNECTION_STRING { get; set; } = "DATA_BASE_CONNECTION_STRING";
        public static string IS_DATA_BASE_ENABLE { get; set; } = "IS_DATA_BASE_ENABLE";
        public static string DATA_BASE_TABLENAME { get; set; } = "DATA_BASE_TABLENAME";
        public static string TESTER_NUMBER { get; set; } = "TESTER_NUMBER";
    }

    public static class  FileSearchPattern
    {
        public static string XML  = "*.xml";
        public static string CSV = "*.csv";
    }
    public static class  FileExtension
    {
        public static string XML = ".xml";
        public static string CSV = ".csv";
    }
}