
public enum GameMode
{
    EditorMode,
    PackageBundle,
    UpdateMode,
}

public class AppConst
{
    public const string BundleExtension = ".ab";
    public const string FileListName = "filelist.txt";

    public static GameMode GameMode = GameMode.EditorMode;

    //�ȸ���Դ·��
    public const string ResourcesUrl = "http://127.0.0.1/AssetBundles";
}