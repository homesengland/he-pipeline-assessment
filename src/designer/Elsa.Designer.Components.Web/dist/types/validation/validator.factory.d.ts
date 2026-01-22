import { Validator, ValidatorEntry } from "./models";
export declare enum Validators {
  Length = "length"
}
export declare function combineValidators<A>(v1: Validator<A>, v2: Validator<A>): Validator<A>;
export declare function getValidator<A>(list: Array<string | ValidatorEntry | Validator<A>>): Validator<A>;
export declare function validatorFactory(name: string, options: any): Validator<any>;
