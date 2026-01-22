export declare enum IconName {
  Plus = "plus",
  TrashBinOutline = "trash-bin-outline",
  OpenInDialog = "open-in-dialog"
}
export declare enum IconColor {
  Blue = "blue",
  Gray = "gray",
  Green = "green",
  Red = "red",
  Default = "currentColor"
}
export interface IconProviderOptions {
  color?: IconColor;
  hoverColor?: IconColor;
}
export declare class IconProvider {
  private map;
  getIcon(name: IconName, options?: IconProviderOptions): any;
}
export declare const iconProvider: IconProvider;
