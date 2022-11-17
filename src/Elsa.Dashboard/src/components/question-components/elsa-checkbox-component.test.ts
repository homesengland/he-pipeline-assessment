//import renderer from 'react-test-renderer';
import { ElsaCheckboxComponent } from './elsa-checkbox-component';

it('componentWillLoad sets a handler for CheckboxEventHandler', () => {
  const component = new ElsaCheckboxComponent();
  expect(component.handler).toBeFalsy();

  component.componentWillLoad();

  expect(component.handler).toBeTruthy();
});
