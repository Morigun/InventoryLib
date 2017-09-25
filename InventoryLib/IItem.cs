/*
 * Сделано в SharpDevelop.
 * Пользователь: Morigun
 * Дата: 21.11.2016
 * Время: 10:26
 */
using System;

namespace InventoryLib
{
	/// <summary>
	/// Description of IItem.
	/// </summary>
	public interface IItem : IEquatable<IItem>, IComparable<IItem>
	{
		/// <summary>
		/// Получение ID для идентефикации предмета
		/// </summary>
		/// <returns>Возвращает кол-во не добавленных предметов</returns>
		int getID();
		/// <summary>
		/// Получение количества предметов
		/// </summary>
		/// <returns>Возвращает кол-во предметов</returns>
		int getCount();
		/// <summary>
		/// Увеличить кол-во предметов
		/// </summary>
		/// <param name="cnt">Количество</param>
		void addCount(int cnt);
		/// <summary>
		/// Уменьшить кол-во предметов
		/// </summary>
		/// <param name="cnt">Количество</param>
		void minusCount(int cnt);
		/// <summary>
		/// Количество привести к максимуму в стэке
		/// </summary>
		void countToMax();
		/// <summary>
		/// Максимальное кол-во предметов в стэке
		/// </summary>
		/// <returns></returns>
		int getMaxInStack();
		/// <summary>
		/// Проверка на стэкаемость предмета
		/// </summary>
		/// <returns>Истина - стэкаемый</returns>
		bool getStacked();
		/// <summary>
		/// Получить стоимость
		/// </summary>
		/// <returns>Стоимость</returns>
		float getCost();
		/// <summary>
		/// Создание нового предмета на основе существуюущего, с указание кол-ва
		/// </summary>
		/// <param name="itemIn">Предмет на основе которого будет создан новый</param>
		/// <param name="count">Количество</param>
		/// <returns>Новый предмет с указанным количеством</returns>
		IItem initItem(int count);
		/// <summary>
		/// Получить значение для сортировки
		/// </summary>
		/// <returns>Значение типа элемента</returns>
		int getValForSort();
	}
}
