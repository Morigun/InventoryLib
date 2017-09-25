/*
 * Сделано в SharpDevelop.
 * Пользователь: Morigun
 * Дата: 22.11.2016
 * Время: 16:51
 */
using System;

namespace InventoryLib
{
	/// <summary>
	/// Description of Purse.
	/// </summary>
	public class Purse
	{
		private float _countMoney;
		public float CountMoney{
			get { return this._countMoney; }
			private set { this._countMoney = value; }
		}
		public Purse()
		{
			this.CountMoney = 0;
		}
		
		public void AddMoney(float cnt){
			if(cnt > 0)
				this.CountMoney += cnt;
		}
		
		public void MinusMoney(float cnt){
			if(cnt > 0 && this.CountMoney > 0)
				this.CountMoney -= cnt;
		}
	}
}
