var WorkflowDesignerMode;
(function (WorkflowDesignerMode) {
  WorkflowDesignerMode[WorkflowDesignerMode["Edit"] = 0] = "Edit";
  WorkflowDesignerMode[WorkflowDesignerMode["Instance"] = 1] = "Instance";
  WorkflowDesignerMode[WorkflowDesignerMode["Blueprint"] = 2] = "Blueprint";
  WorkflowDesignerMode[WorkflowDesignerMode["Test"] = 3] = "Test";
})(WorkflowDesignerMode || (WorkflowDesignerMode = {}));
var LayoutDirection;
(function (LayoutDirection) {
  LayoutDirection["LeftRight"] = "leftright";
  LayoutDirection["TopBottom"] = "topbottom";
  LayoutDirection["RightLeft"] = "rightleft";
  LayoutDirection["BottomTop"] = "bottomtop";
})(LayoutDirection || (LayoutDirection = {}));

export { LayoutDirection as L, WorkflowDesignerMode as W };
