# Android Closed Testing

Pocket Factory includes an `export_presets.cfg.example` template for the first Google Play closed-test preset. Godot's live `export_presets.cfg` stays ignored because it can contain signing credentials.

The project uses `com.theexploringduck.pocketfactory` as its proposed Android application id. Confirm that this id is available in Google Play Console before the first upload: the id cannot be changed for an already-published app.

## One-time machine setup

1. Install Android Studio and complete its first-run setup.
2. In SDK Manager, install Android SDK Platform-Tools 35+, Build-Tools 35.0.1, Android Platform 35, Command-line Tools, CMake 3.10.2.4988404, and NDK 28.1.13356709.
3. In Godot Editor Settings, set the Java SDK path to the installed JDK 17 and Android SDK path to the Android SDK directory.
4. In Godot, open `Project > Manage Export Templates`, download the templates for the installed Godot version, then select `Project > Install Android Build Template`.
5. Create a release upload key outside this repository, configure it in the `Android Closed Test` export preset, and keep the key plus its passwords in a password manager. Never commit the key or credentials.

## Export

1. Open `game/PocketFactory.Godot` using the Godot .NET editor.
2. Select `Project > Export`, then `Android Closed Test`.
3. Configure the preset from `export_presets.cfg.example`, confirm `Use Gradle Build` and `Android App Bundle (.aab)` are selected, then add the release-key details locally.
4. Export a release build without the debug option enabled.
5. Upload the resulting `game/PocketFactory.Godot/build/PocketFactory-closed-test.aab` to the Google Play closed-testing track.

Godot's Android C# export remains experimental, so install the generated AAB through a Play closed test before treating an export as release-ready.
