# Android Closed Testing

Pocket Factory includes an `export_presets.cfg.example` template for the first Google Play closed-test preset. Godot's live `export_presets.cfg` stays ignored because it can contain signing credentials.

The project uses `com.theexploringduck.pocketfactory` as its proposed Android application id. Confirm that this id is available in Google Play Console before the first upload: the id cannot be changed for an already-published app.

## Google Play target requirement

As of August 31, 2026, new phone/tablet apps and app updates submitted to Google Play must target Android 16 / API level 36 or higher. Before the first closed-test upload, confirm the installed Godot Android toolchain and generated Gradle project target API 36 or newer. Do not rely on an older API 35 setup simply because it builds locally.

The current Godot 4.5.1 .NET Android template is configured and tested by Godot at API 35. It can produce a locally signed debug APK for device smoke testing, but that APK is not eligible for a Play upload. Upgrade to a Godot .NET Android export environment with a tested API 36 template before creating the first closed-test AAB.

## One-time machine setup

1. Install Android Studio and complete its first-run setup.
2. In SDK Manager, install Android SDK Platform-Tools, Android Platform 36 (or newer supported by the installed Godot version), a compatible current Build-Tools package, Command-line Tools, and the CMake/NDK versions required by the installed Godot release.
3. In Godot Editor Settings, set the Java SDK path to the installed JDK 17 and Android SDK path to the Android SDK directory.
4. In Godot, open `Project > Manage Export Templates`, download the templates for the installed Godot version, then select `Project > Install Android Build Template`.
5. Create a release upload key outside this repository, configure it in the `Android Closed Test` export preset, and keep the key plus its passwords in a password manager. Never commit the key or credentials.

## Export

1. Open `game/PocketFactory.Godot` using the Godot .NET editor.
2. Select `Project > Export`, then `Android Closed Test`.
3. Configure the preset from `export_presets.cfg.example`, confirm `Use Gradle Build` and `Android App Bundle (.aab)` are selected, then add the release-key details locally.
4. Confirm the resulting Android build targets API level 36 or higher before upload.
5. Export a release build without the debug option enabled.
6. Upload the resulting `game/PocketFactory.Godot/build/PocketFactory-closed-test.aab` to the Google Play closed-testing track.

Godot's Android C# export remains experimental, so install the generated AAB through a Play closed test before treating an export as release-ready.
