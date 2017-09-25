/*
 * Сделано в SharpDevelop.
 * Пользователь: Morigun
 * Дата: 21.11.2016
 * Время: 10:26
 */
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace InventoryLib
{
	/// <summary>
	/// Description of InventoryClass.
	/// </summary>
	[Serializable]
	public class InventoryClass
	{
		List<IItem> items;		
		int size;
		Purse purse;
		
		#region .ctor
		/// <summary>
		/// Конструктор, принимает кол-во ячеек, максимальное, которое может быть в инвентаре
		/// </summary>
		/// <param name="size">Ограничение по кол-ву ячеек</param>
		public InventoryClass(int size)
		{
			this.size = size;
			this.items = new List<IItem>();
			//Инициализируем все ячейки инвентаря
			for(int i = 0; i < size; i++){
				this.items.Add(null);
			}
			this.purse = new Purse();
		}
		#endregion
		
		#region PUBLIC
		/// <summary>
		/// Добавить предмет в инвентарь
		/// </summary>
		/// <param name="itemIn">Предмет</param>
		/// <returns>Кол-во не добавленых элементов</returns>
		public int Add(IItem itemIn){
			int ost = itemIn.getCount();
			IItem tmpItem = findItem(itemIn);
			while(ost != 0 && tmpItem != null){ // Сначала ищем все ячейки с таким предметов в которых есть место
				if(tmpItem.getCount() + ost < tmpItem.getMaxInStack()){//Полное добавление
					tmpItem.addCount(ost);
					ost = 0;
				} else {//Частичное добавление
					ost -= tmpItem.getMaxInStack() - tmpItem.getCount();
					tmpItem.countToMax();
				}
				tmpItem = findItem(itemIn);//Ищем следующую ячейку с таким предметом
			}
			if(ost != 0){//Если не удалось полностью добавить в инвентарь текущий предмет
				tmpItem = findAddItem(itemIn);//Ищем и добавляем элемент
				if(tmpItem != null)
					ost = 0;
			}
			return ost;
		}
		/// <summary>
		/// Удалить предмет из инвентаря
		/// </summary>
		/// <param name="itemIn">Предмет</param>
		/// <returns>Кол-во не удаленных предметов</returns>
		public int Del(IItem itemIn){
			int ost = itemIn.getCount();
			IItem tmpItem = findItem(itemIn);//Ищем в инвентаре ячейку с данным предметом
			while(ost != 0 && tmpItem != null){//Проверяем, что кол-во элементов для удаления больше нуля и ячейка с предметом найдена
				if(tmpItem.getCount() - ost > 0){//Уменьшение количества элементов в ячейке на все указанное количество
					tmpItem.minusCount(ost);
					ost = 0;
				} else {//Уменьшение количества элементов в ячейке по частям
					if(tmpItem.getCount() - ost == 0){//Полное удаление из ячейки с обнулением указанного количества
						tmpItem = null;
						ost = 0;
					} else {//Полное удаление из ячейки с уменьшением указанного количества на удаленное количество ячейки
						ost -= tmpItem.getCount();
						tmpItem = null;						
					}
				}
				tmpItem = findItem(itemIn);//Ищем следующий элемент
			}
			return ost;
		}
		/// <summary>
		/// Продать предмет
		/// </summary>
		/// <param name="itemIn">Предмет</param>
		/// <returns>Кол-во не проданных предметов</returns>
		public int SellItem(IItem itemIn){
			int ost = Del(itemIn);
			addMoney(itemIn.getCost() * (itemIn.getCount() - ost));
			return ost;
		}
		/// <summary>
		/// Купить предмет
		/// </summary>
		/// <param name="itemIn">Предмет</param>
		/// <returns>Кол-во не купленых предметов</returns>
		public int BuyItem(IItem itemIn){
			int countOnCost = checkMoney(itemIn.getCount(), itemIn.getCost());
			int ost = 0;
			if(countOnCost > 0){
				ost = Add(itemIn.initItem(countOnCost));
				minusMoney((itemIn.getCount() - ost) * itemIn.getCost());
			}
			else
				return itemIn.getCount();
			return ost;
		}
		/// <summary>
		/// Получить кол-во денег в кошельке
		/// </summary>
		/// <returns>Кол-во денег</returns>
		public float getMoney(){
			return this.purse.CountMoney;
		}
		/// <summary>
		/// Добавить деньги в кошелек
		/// </summary>
		/// <param name="money">Кол-во денег</param>
		/// <returns></returns>
		public void addMoney(float money){
			this.purse.AddMoney(money);
		}
		/// <summary>
		/// Убавить деньги в кошельке
		/// </summary>
		/// <param name="money">Кол-во денег</param>
		public void minusMoney(float money){
			this.purse.MinusMoney(money);
		}
		/// <summary>
		/// Кол-во не null ячеек
		/// </summary>
		/// <returns>Кол-во ячеек</returns>
		public int getCount(){
			int cnt = 0;
			foreach(IItem item in this.items){
				if(item != null)
					cnt++;
			}
			return cnt;
		}
		/// <summary>
		/// Получить предмет
		/// </summary>
		/// <param name="num">Номер ячейки</param>
		/// <returns>Предмет</returns>
		public IItem getElement(int num){
			return this.items[num];
		}
		
		public void Sort(SortType st){
			switch(st){
				case SortType.COST_SORT:
					items.Sort(delegate(IItem x, IItem y)
					{
						if(x == null && y == null) return 1;
						else if (x == null) return 0;
						else if (y == null) return -1;
						else return y.getCost().CompareTo(x.getCost());
					});
					break;
				case SortType.TYPE_SORT:
					items.Sort(delegate(IItem x, IItem y)
					{
						if(x == null && y == null) return 1;
						else if (x == null) return 0;
						else if (y == null) return -1;
						else return x.getValForSort().CompareTo(y.getValForSort());
					});
					break;
			}
		}
		/// <summary>
		/// Обнулить инвентарь
		/// </summary>
		public void renullInventory(){
			this.items.Clear();
			for(int i = 0; i < size; i++){
				this.items.Add(null);
			}
		}
		#endregion
		
		#region PRIVATE
		/// <summary>
		/// Проверка денег для покупки предмета
		/// </summary>
		/// <param name="count">Кол-во предмета</param>
		/// <param name="costOnOne">Цена за еденицу</param>
		/// <returns>Возможное кол-во к продаже</returns>
		private int checkMoney(int count, float costOnOne){
			if(purse.CountMoney > count * costOnOne)
				return count;
			else{
				if(purse.CountMoney > costOnOne){
					int newCount = 0;
					for(int i = 0; i < count; i++){
						if(!(purse.CountMoney > i * costOnOne))
							break;
						newCount++;
					}
					return newCount;
				}
				else{
					return 0;
				}
			}
		}
		/// <summary>
		/// Поиск номера пустой ячейки
		/// </summary>
		/// <returns>номер пустой ячейки</returns>
		public int findEmpty(){
			int num = 0;
			foreach(IItem item in items){
				if(item == null)
					return num;
				num++;
			}
			return -1;
		}
		/// <summary>
		/// Поиск элемента в инвентаре с учетом максимального кол-ва для добавления нового элемента
		/// </summary>
		/// <param name="itemIN">Предмет для поиска</param>
		/// <returns>Найденый предмет в инвентаре</returns>
		private IItem findItem(IItem itemIN){
			foreach(IItem item in items){
				if(item != null){
					if(item.Equals(itemIN)){
						if(item.getStacked()){
							if(item.getMaxInStack() > itemIN.getCount()){
								return item;
							}
						}
					}
				}
			}
			return null;
		}
		/// <summary>
		/// Поиск и добавление предмета в инвентарь
		/// </summary>
		/// <param name="itemIN">Предмет для добавления</param>
		/// <returns>Добавленый предмет в инвентаре</returns>
		private IItem findAddItem(IItem itemIN){
			if(this.getCount() < size){
				int cellElement = findEmpty();
				this.items[cellElement] = itemIN.initItem(itemIN.getCount());
				return this.items[cellElement];
			}
			return null;
		}
		#endregion
	}
}
