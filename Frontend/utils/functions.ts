import { shuffle, range } from "lodash";

export function createArray(start: number, end: number): number[] {
  return range(start, end);
}

export function getListWithRandomItems<TItem>(
  itemList: TItem[],
  n: number,
): TItem[] {
  const shuffledItemList = shuffle(itemList);

  if (itemList.length < n) return shuffledItemList;

  const randomList = createArray(0, n).map(
    (_, index) => shuffledItemList[index],
  );
  return randomList;
}
