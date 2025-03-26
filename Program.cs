using System;
using System.Threading;
using DiscordRPC;
using XPression;  // Ensure you have added the Ross XPression API reference

namespace XPressionDiscordRichPresence
{
    class Program
    {
        static DiscordRpcClient discordClient;
        static xpEngine Engine = new xpEngine();

        static void Main(string[] args)
        {

            // Initialize Discord RPC client with your Discord Application Client ID
            discordClient = new DiscordRpcClient("1354484297737961542");

            discordClient.OnReady += (sender, e) =>
            {
                Console.WriteLine($"Connected to Discord as {e.User.Username}");
            };

            discordClient.Initialize();

            // Set an initial presence
            UpdatePresence("Idle", "No focused object");

            // Main loop: update rich presence every 5 seconds
            while (true)
            {
                xpScene scene;
                Engine.GetFocusedScene(out scene);
                string focusedObjectDetails = GetFocusedObjectDetails(scene);

                UpdatePresence(scene.Name, focusedObjectDetails);

                Thread.Sleep(5000);  // Adjust interval as needed
            }
        }

        /// <summary>
        /// Updates the Discord rich presence with the given scene and focused object details.
        /// </summary>
        static void UpdatePresence(string scene, string focusedObject)
        {

            discordClient.SetPresence(new RichPresence()
            {
                Details = $"Working on: {scene}",
                State = $"Focusing on: {focusedObject}",
                Timestamps = Timestamps.Now,
                Assets = new Assets()
                {
                    // Ensure these keys match the assets uploaded in your Discord Developer Portal
                    LargeImageKey = "your_large_image_key",
                    LargeImageText = "Ross XPression"
                }
            });
        }

        /// <summary>
        /// Retrieves the name of the focused scene from the XPression engine.
        /// </summary>
        static string GetFocusedScene()
        {
            // Example: using a property or method from the engine to get the current scene.
            // Adjust based on your actual Ross XPression API implementation.
            xpScene scene;
            Engine.GetFocusedScene(out scene);  // Alternatively: xpressionEngine.GetFocusedScene();
            return scene != null ? scene.Name : "No Scene";
        }

        /// <summary>
        /// Retrieves details (type and name) of the focused object from the XPression engine.
        /// </summary>
        static string GetFocusedObjectDetails(xpScene scene)
        {
            // Example: using a property or method from the engine to get the focused object.
            // Adjust based on your actual Ross XPression API implementation.
            xpBaseObject focusedObject;
            scene.GetFocusedObject(out focusedObject);  // Alternatively: xpressionEngine.GetFocusedObject();
            if (focusedObject != null)
            {
                return $"{focusedObject.TypeName}: {focusedObject.Name}";
            }
            return "No Object";
        }
    }
}
