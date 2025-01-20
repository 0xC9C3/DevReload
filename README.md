# DevReload for DPB2

A small plugin to watch a dll for changes and reload it. By default it watches a "dev.dll" inside the DPB root dir. Can be configured inside the settings json file (Settings/Default/DevReload.json).

Place the plugin into the Plugins directory and start DPB.

New interface implementations (new Bot, Mover, Plugin or Routine) do not get loaded until a DPB restart.

Initialize and Deinitialize will be called on reload. If the Bot was running, it will restart it on reload.

This is very hacky, but improved my iteration speed a lot.

