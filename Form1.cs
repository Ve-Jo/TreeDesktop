using System.IO;

namespace ThreeViewDesktop
{
    public partial class Form1 : Form
    {
        private Dictionary<string, int> imageIndexCache;

        public Form1()
        {
            InitializeComponent();
            imageIndexCache = new Dictionary<string, int>();
            InitializeTreeView();
        }

        private void InitializeTreeView()
        {
            imageList = new ImageList();
            imageList.ImageSize = new Size(16, 16);
            treeView1.ImageList = imageList;

            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            PopulateTreeView(treeView1.Nodes, desktopPath);
        }

        private void PopulateTreeView(TreeNodeCollection nodes, string path)
        {
            try
            {
                foreach (string directoryPath in Directory.GetDirectories(path))
                {
                    TreeNode directoryNode = new TreeNode(Path.GetFileName(directoryPath));
                    directoryNode.ImageIndex = LoadIconIndexFromFile(directoryPath);
                    directoryNode.SelectedImageIndex = directoryNode.ImageIndex;
                    nodes.Add(directoryNode);
                    PopulateTreeView(directoryNode.Nodes, directoryPath);
                    LoadFiles(directoryNode, directoryPath);
                }
            }
            catch (UnauthorizedAccessException)
            {
            }
        }

        private void LoadFiles(TreeNode parentNode, string directoryPath)
        {
            try
            {
                foreach (string filePath in Directory.GetFiles(directoryPath))
                {
                    string fileName = Path.GetFileName(filePath);
                    TreeNode fileNode = new TreeNode(fileName);
                    fileNode.ImageIndex = LoadIconIndexFromFile(filePath);
                    fileNode.SelectedImageIndex = fileNode.ImageIndex;
                    parentNode.Nodes.Add(fileNode);
                }
            }
            catch (UnauthorizedAccessException)
            {
            }
        }

        private int LoadIconIndexFromFile(string filePath)
        {
            try
            {
                bool isDirectory = Directory.Exists(filePath);
                string key = isDirectory ? "folder" : "file";

                if (imageIndexCache.ContainsKey(key))
                {
                    return imageIndexCache[key];
                }
                else
                {
                    Icon icon = isDirectory ? SystemIcons.Asterisk : SystemIcons.Application;
                    imageList.Images.Add(icon);
                    int imageIndex = imageList.Images.Count - 1;
                    imageIndexCache[key] = imageIndex;
                    return imageIndex;
                }
            }
            catch (Exception ex)
            {
            }

            return 0;
        }
    }
}