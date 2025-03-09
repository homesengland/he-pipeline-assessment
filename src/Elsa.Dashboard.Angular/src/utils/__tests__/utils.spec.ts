import { format, isNumeric } from '../utils';

describe('utils', () => {
  describe('format() function tests', () => {
    const firstName = 'John';
    const middleName = 'Dimello';
    const lastName = 'Doe';

    it('supplying first name, middle name and last name returns correctly formatted string', () => {
      expect(format(firstName, middleName, lastName)).toEqual('John Dimello Doe');
    });
    it('supplying first name and last name returns correctly formatted string', () => {
      expect(format(firstName, '', lastName)).toEqual('John Doe');
    });
    it('supplying first name only returns correctly formatted string', () => {
      expect(format(firstName, '', '')).toEqual('John');
    });
    it('supplying last name only returns correctly formatted string', () => {
      expect(format('', '', lastName)).toEqual(' Doe');
    });
    it('supplying no parameters returns correctly empty string', () => {
      expect(format('', '', '')).toEqual('');
    });
    it('foo test', () => {
      expect(1).toEqual(1);
    });
  });

  describe('isNumeric() function tests', () => {
    it('supplying a number as a string returns true', () => {
      expect(isNumeric('2')).toEqual(true);
    });
    it('supplying a number as a word returns false', () => {
      expect(isNumeric('two')).toEqual(false);
    });
    it('not supplying a number at all returns false', () => {
      expect(isNumeric('')).toEqual(false);
    });
  });
});
