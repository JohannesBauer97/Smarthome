import { TimeDiffInSecondsPipe } from './time-diff-in-seconds.pipe';

describe('TimeDiffInSecondsPipe', () => {
  it('create an instance', () => {
    const pipe = new TimeDiffInSecondsPipe();
    expect(pipe).toBeTruthy();
  });
});
