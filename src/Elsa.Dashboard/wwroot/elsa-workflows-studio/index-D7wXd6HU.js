import './events-CpKc8CLe.js';

var WorkflowContextFidelity;
(function (WorkflowContextFidelity) {
    WorkflowContextFidelity["Burst"] = "Burst";
    WorkflowContextFidelity["Activity"] = "Activity";
})(WorkflowContextFidelity || (WorkflowContextFidelity = {}));
var WorkflowPersistenceBehavior;
(function (WorkflowPersistenceBehavior) {
    WorkflowPersistenceBehavior["Suspended"] = "Suspended";
    WorkflowPersistenceBehavior["WorkflowBurst"] = " WorkflowBurst";
    WorkflowPersistenceBehavior["WorkflowPassCompleted"] = "WorkflowPassCompleted";
    WorkflowPersistenceBehavior["ActivityExecuted"] = "ActivityExecuted";
})(WorkflowPersistenceBehavior || (WorkflowPersistenceBehavior = {}));
var WorkflowStatus;
(function (WorkflowStatus) {
    WorkflowStatus["Idle"] = "Idle";
    WorkflowStatus["Running"] = "Running";
    WorkflowStatus["Finished"] = "Finished";
    WorkflowStatus["Suspended"] = "Suspended";
    WorkflowStatus["Faulted"] = "Faulted";
    WorkflowStatus["Cancelled"] = "Cancelled";
})(WorkflowStatus || (WorkflowStatus = {}));
var OrderBy;
(function (OrderBy) {
    OrderBy["Started"] = "Started";
    OrderBy["LastExecuted"] = "LastExecuted";
    OrderBy["Finished"] = "Finished";
})(OrderBy || (OrderBy = {}));
var ActivityTraits;
(function (ActivityTraits) {
    ActivityTraits[ActivityTraits["Action"] = 1] = "Action";
    ActivityTraits[ActivityTraits["Trigger"] = 2] = "Trigger";
    ActivityTraits[ActivityTraits["Job"] = 4] = "Job";
})(ActivityTraits || (ActivityTraits = {}));
class SyntaxNames {
}
SyntaxNames.Literal = 'Literal';
SyntaxNames.JavaScript = 'JavaScript';
SyntaxNames.Liquid = 'Liquid';
SyntaxNames.Json = 'Json';
SyntaxNames.Variable = 'Variable';
SyntaxNames.Output = 'Output';
const getVersionOptionsString = (versionOptions) => {
    if (!versionOptions)
        return '';
    return versionOptions.allVersions
        ? 'AllVersions'
        : versionOptions.isDraft
            ? 'Draft'
            : versionOptions.isLatest
                ? 'Latest'
                : versionOptions.isPublished
                    ? 'Published'
                    : versionOptions.isLatestOrPublished
                        ? 'LatestOrPublished'
                        : versionOptions.version.toString();
};
var WorkflowTestActivityMessageStatus;
(function (WorkflowTestActivityMessageStatus) {
    WorkflowTestActivityMessageStatus["Done"] = "Done";
    WorkflowTestActivityMessageStatus["Waiting"] = "Waiting";
    WorkflowTestActivityMessageStatus["Failed"] = "Failed";
    WorkflowTestActivityMessageStatus["Modified"] = "Modified";
})(WorkflowTestActivityMessageStatus || (WorkflowTestActivityMessageStatus = {}));

const FlowchartEvents = {
    ConnectionCreated: 'connection-created'
};

export { ActivityTraits as A, FlowchartEvents as F, OrderBy as O, SyntaxNames as S, WorkflowContextFidelity as W, WorkflowPersistenceBehavior as a, WorkflowStatus as b, WorkflowTestActivityMessageStatus as c, getVersionOptionsString as g };
//# sourceMappingURL=index-D7wXd6HU.js.map

//# sourceMappingURL=index-D7wXd6HU.js.map