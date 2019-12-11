Create trigger ActualizarStock_Ingreso
on detalle_ingreso
for insert
as 
update a Set a.stock=a.stock+d.cantidad
from articulo a inner join 
inserted as d on d.idarticulo=a.idarticulo