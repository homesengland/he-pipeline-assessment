import { createStore } from "@stencil/store";

const { state, onChange } = createStore({
  dictionaryGroups: []

});

onChange("dictionaryGroups", value => { state.dictionaryGroups = value });

export default state;

