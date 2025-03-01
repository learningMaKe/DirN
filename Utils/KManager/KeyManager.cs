#define REFACTOR
using DirN.Utils.Events.EventType;
using DirN.Utils.Extension;
using DirN.Utils.KManager.HKey;
using DirN.Utils.KManager.HMouseButton;
using DirN.Utils.KManager.HMouseWheel;
using DirN.Utils.Maps;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text.Json.Serialization;
using System.Windows;
using System.Windows.Input;

namespace DirN.Utils.KManager
{
    public class KeyManager : ManagerBase<KeyManager>
    {
        private const string DefaultPath = "./Settings/Map.json";

        private MapHandlerGroup mapHandlers = new();

        public bool RequiredSave = false;

        public KeyManager(IContainerProvider containerProvider) : base(containerProvider)
        {
            Initilize();
            MainManager mainManager = containerProvider.Resolve<MainManager>();
            mainManager.WindowClosed += OnWindowClosed;
        }

        public void InvokeElementEvent<TArgs>(TArgs e, object key) where TArgs : EventArgs
        {
            mapHandlers.Invoke(key, e);
        }

        public void RegisterEvent<TArgs>(EventId eventId, Action<TArgs>? action = null) where TArgs:EventArgs
        {
            mapHandlers.RegisterEvent(eventId, action);
        }

        public void ChangeMap<TKey>(EventId eventId, TKey newKey) where TKey:notnull
        {
            RequiredSave = true;
            mapHandlers.ChangeMap(eventId, newKey);
        }

        public void SaveKeyMap(bool userSelect = false)
        {
            string path = string.Empty;
            if(userSelect)
            {
                Microsoft.Win32.SaveFileDialog dialog = new()
                {
                    FileName = "KeyMap.json",
                    Filter = "JSON File (*.json)|*.json"
                };
                if (dialog.ShowDialog() == true)
                {
                    path = dialog.FileName;
                }
            }
            // ToDo : Save to file
            if (string.IsNullOrEmpty(path))
            {
                path = DefaultPath;
            }
            string? directory = Path.GetDirectoryName(path);
            if (string.IsNullOrEmpty(directory)) return;
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            string json = JsonConvert.SerializeObject(mapHandlers.Export(), Formatting.Indented);
            File.WriteAllText(path, json);
        }

        public void UserInit()
        {
            RequiredSave = true;
            mapHandlers = Mapper<DefaultMapManager, MapHandlerGroup>.Instance;
        }

        private void OnWindowClosed(CancelEventArgs e)
        {
            if (RequiredSave)
            {
                SaveKeyMap();
            }
        }

        private void Initilize()
        {
            IList<(Type, IList<(EventId, object)>)>? maps = null;
            #if !REFACTOR
            try
            {
                string text = File.ReadAllText(DefaultPath);
                maps = JsonConvert.DeserializeObject<IList<(Type,IList<(EventId,object)>)>>(text);
                if (maps == null) throw new Exception();
                mapHandlers.Init(maps);
            }
            catch
            {
                maps = null;
            }
            #endif
            if(maps == null)
            {
                mapHandlers = Mapper<DefaultMapManager, MapHandlerGroup>.Instance;
                RequiredSave = true;
            }
        }

    }
}
