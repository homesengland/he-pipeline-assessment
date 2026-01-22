import { ElsaPlugin } from "../services";
import { ActivityEditorAppearingEventArgs, ActivityEditorDisappearingEventArgs } from "../components/screens/workflow-definition-editor/elsa-activity-editor-modal/elsa-activity-editor-modal";
export declare class SendHttpRequestPlugin implements ElsaPlugin {
  constructor();
  onActivityEditorAppearing: (args: ActivityEditorAppearingEventArgs) => void;
  onActivityEditorDisappearing: (args: ActivityEditorDisappearingEventArgs) => void;
  updateUI: () => void;
}
