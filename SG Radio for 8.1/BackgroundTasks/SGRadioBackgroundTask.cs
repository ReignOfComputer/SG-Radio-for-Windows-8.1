using Windows.ApplicationModel.Background;
using Windows.UI.StartScreen;

namespace BackgroundTasks
{
    public sealed class SGRadioBackgroundTask : IBackgroundTask
    {
        private readonly string[] titleId = { "4000", "4001", "4002", "4003", "4004", "4005", "4006", "4007", "4008", "4009", "4010", "4011", "4012", "4013", "4014",
            "5000", "5001", "5002", "5003", "5004", "5005", "5006", "5007", "5008", "5009", "5010", "5011", "5012" };

        public void Run(IBackgroundTaskInstance taskInstance)
        {
            BackgroundTaskDeferral deferral = taskInstance.GetDeferral();

            Windows.UI.Notifications.TileUpdateManager.CreateTileUpdaterForApplication().Clear();

            foreach (string titleId in titleId)
            {
                if (SecondaryTile.Exists(titleId))
                {
                    Windows.UI.Notifications.TileUpdateManager.CreateTileUpdaterForSecondaryTile(titleId).Clear();
                }
            }

            deferral.Complete();
        }
    }
}
