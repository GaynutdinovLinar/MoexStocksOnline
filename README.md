# MoexStocksOnline

MoexStockOnline - библиотека, позволяющая получать информацию (в частности их стоимость) о ценных бумагах, которые есть на Московской бирже, в онлайн режиме (15-минутная задержка).

Moex - главный класс, содержит метод позволяющий получить список возможных видов торгов.
Board - вид торга. Содержит метод, позволяющий получить список ценных бумаг по данному виду.
Stock - ценная бумага. Имеет метод для получения стоимости данной ценной бумаги за промежуток времени в виде коллекции DayCost.
