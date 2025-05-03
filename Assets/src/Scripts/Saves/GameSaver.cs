public class GameSaver
{
    public static GameSaver Instance;

    public Data Data { get; private set; }

    private static string dataSaveName = "Saves";

    public GameSaver()
    {
        Instance = this;
        Data = ExcelConverter.ImportFromExcel("D:\\Unity\\UnityProjects\\KiberOneLearningApp\\Assets\\Кибероны Ученики.xlsx");
        
        if (Data == null)
            Data = new Data();
    }

    public static void Save() => DataParser.Save(dataSaveName, Instance.Data);

    public Data Load() => Data = DataParser.Load(dataSaveName);
}