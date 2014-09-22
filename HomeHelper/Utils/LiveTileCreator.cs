using System;
using HomeHelper.Model;
using HomeHelper.Model.Abstract;
using HomeHelper.Repository.Abstract;
using HomeHelper.Repository.Concret;
using Windows.ApplicationModel.Resources;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;

namespace HomeHelper.Utils
{
    public class LiveTileCreator
    {
        private IRepository<Utilitati> _repositoryUtilitati;
        private IRepository<ConsumUtilitate> _repositoryConsum;
        private IRepository<AlertaUtilitate> _repositoryAlerta;
        private Random _aleator;
        private bool _isAlive;

        public LiveTileCreator()
        {
            _repositoryAlerta = new AlertaUtilitateRepository();
            _repositoryConsum = new ConsumUtilitateRepository();
            _repositoryUtilitati = new UtilitatiRepository();
            _aleator = new Random();
        }

        public int LiveTileTtl { get; set; }


        public void SetupTileNotificationWide()
        {
            var loader = new Windows.ApplicationModel.Resources.ResourceLoader();
            var document = BuildTileXml(TileTemplateType.TileWideText01);
            var nodes = document.GetElementsByTagName("text");
            var list = _repositoryUtilitati.GetAll();
            _isAlive = true;
            if (list.Count == 0) return;
            var i = _aleator.Next(0, list.Count - 1);
            nodes[0].InnerText = "HomeHelper";
            nodes[1].InnerText = list[i].DenumireUtilitate;
            nodes[2].InnerText = string.Format("{2} {0} {1}", list[i].ConsumActual, list[i].UnitateMasura, loader.GetString(resource: "ConsumLunaCurentaMesaj"));
            nodes[3].InnerText = string.Format("{2} {0} {1}", list[i].ConsumLunaAnterioara, list[i].UnitateMasura, loader.GetString(resource: "ConsumaLunaAnterioaraMesaj"));
            ShowUpTileNotification(document);
        }

        public void SetupTileNotificationSquare()
        {
            
        }

        private void ShowUpTileNotification(XmlDocument xml)
        {
            var notificati = new TileNotification(xml);
            notificati.ExpirationTime = DateTimeOffset.UtcNow.AddSeconds(LiveTileTtl);
            var updater = TileUpdateManager.CreateTileUpdaterForApplication();
            updater.Update(notificati);
            
        }

        private XmlDocument BuildTileXml(TileTemplateType type)
        {
            return TileUpdateManager.GetTemplateContent(type);
        }
    }


    public class WindowsResources:IResources
    {
        private readonly ResourceLoader _loader;
        public WindowsResources()
        {
            _loader = new ResourceLoader();
        }
        public string GetString(string resource)
        {
            return _loader.GetString(resource);
        }
    }
}