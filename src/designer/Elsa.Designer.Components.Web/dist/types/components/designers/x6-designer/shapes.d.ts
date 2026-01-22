import { Shape } from '@antv/x6';
import { ActivityModel, ActivityDescriptor, ActivityDesignDisplayContext } from '../../../models';
export declare class ActivityNodeShape extends Shape.HTML {
  get component(): string;
  set component(value: string);
  set activity(value: ActivityModel);
  get activity(): ActivityModel;
  get activityDescriptor(): ActivityDescriptor;
  set activityDescriptor(value: ActivityDescriptor);
  get displayContext(): ActivityDesignDisplayContext;
  set displayContext(value: ActivityDesignDisplayContext);
  init(): void;
  setup(): void;
  updateSize(): void;
  createHtml(): string;
}
