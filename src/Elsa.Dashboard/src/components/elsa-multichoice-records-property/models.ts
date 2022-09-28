// // import {Map} from "../../../../../Elsa-Core-HE/src/designer/elsa-workflows-studio/src/utils/utils";
// type Map<T> = {
//     [key: string]: T
//   };

// export interface MultiChoiceRecord {
//     name: string;
//     expressions?: Map<string>;
//     syntax?: string;
// }

export interface MultiChoiceRecord {
    answer: string;
    isSingle: boolean;
}