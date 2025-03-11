import { format, isNumeric } from '../utils';

describe('utils', () => {
  describe('format', () => {
    let firstName;
    let middleName;
    let lastName;

    beforeEach(() => {
      firstName = 'John';
      middleName = 'Dimello';
      lastName = 'Doe';
    });

    it('should return correctly formatted string when first name, middle name and last name is provided', () => {
      expect(format(firstName, middleName, lastName)).toEqual('John Dimello Doe');
    });
    it('should return correctly formatted string when first name and last name is provided', () => {
      expect(format(firstName, '', lastName)).toEqual('John Doe');
    });
    it('should return correctly formatted string when first name is only provided', () => {
      expect(format(firstName, '', '')).toEqual('John');
    });
    it('should return correctly formatted string when last name is only provided', () => {
      expect(format('', '', lastName)).toEqual(' Doe');
    });
    it('should return empty formatted string when no parameters are provided', () => {
      expect(format('', '', '')).toEqual('');
    });
  });

  describe('isNumeric', () => {
    it('should return true when providing a number as a string', () => {
      expect(isNumeric('2')).toEqual(true);
    });
    it('should return false when providing a number as a word', () => {
      expect(isNumeric('two')).toEqual(false);
    });
    it('should return false when not providing a number as a string', () => {
      expect(isNumeric('')).toEqual(false);
    });
  });
});
