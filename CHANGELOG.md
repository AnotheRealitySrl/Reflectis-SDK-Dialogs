# Release notes

## v1.1.0

### Added

- DialogNode: Added enum LayoutElementSide to choose wich side to place Nickname and Avatar.
- DialogNode: Added fields characterNameSide and characterAvatarSide of type LayoutElementSide.
- DialogPanelController: Added float fields to define X position deltas for Player and NPC dialogs, both for left or right side of Nickname and Avatar.

### Changed

- DialogNode: Changed fields definition order.
- DialogPanelControllerGeneric: Added new side parameters in SetNicknameText and SetAvatar methods.
- DialogPanelController: Added override method SetNicknameText to use the new X position deltas related to the chosen side, both for Player and NPC.
- DialogPanelController: Updated override method SetAvatar to use the new X position deltas related to the chosen side, both for Player and NPC.
- DialogPanelControllerEditor: Adapted to new fields.
- DialogPanel.prefab: Added default X position deltas for nicknames and avatars of Player and NPC boxes.

## v1.0.0

- Initial release
