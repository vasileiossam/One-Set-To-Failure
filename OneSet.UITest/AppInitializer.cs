using System;
using System.IO;
using System.Linq;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace OneSet.UITest
{
    public class AppInitializer
    {
        public static IApp StartApp(Platform platform)
        {
            if (platform == Platform.Android)
            {
                return ConfigureApp
                    .Android
                    .ApkFile ("../../../Droid/bin/Release/com.vasileiossam.oneset.apk")
                    .EnableLocalScreenshots()
                    .StartApp();
            }

            return null;

            //return ConfigureApp
            //    .iOS
            // TODO: Update this path to point to your iOS app and uncomment the
            // code if the app is not included in the solution.
            //.AppBundle ("../../../iOS/bin/iPhoneSimulator/Debug/XamarinForms.iOS.app")
            //.DeviceIdentifier("F2CA162A-15A5-4023-B97A-427A2A9B59B7")
            //.CodesignIdentity("iPhone Developer: Rob DeRosa (VH66B6QF79)")
            //.StartApp();
        }
    }
}

