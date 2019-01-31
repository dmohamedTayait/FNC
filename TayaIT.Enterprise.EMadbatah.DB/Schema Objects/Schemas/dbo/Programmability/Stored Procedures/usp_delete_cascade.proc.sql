create procedure usp_delete_cascade (
        @base_table_name varchar(200), @base_criteria nvarchar(1000)
)
as begin
        -- Adapted from http://www.sqlteam.com/article/performing-a-cascade-delete-in-sql-server-7
        -- Expects the name of a table, and a conditional for selecting rows
        -- within that table that you want deleted.
        -- Produces SQL that, when run, deletes all table rows referencing the ones
        -- you initially selected, cascading into any number of tables,
        -- without the need for "ON DELETE CASCADE".
        -- Does not appear to work with self-referencing tables, but it will
        -- delete everything beneath them.
        -- To make it easy on the server, put a "GO" statement between each line.

        declare @to_delete table (
                id int identity(1, 1) primary key not null,
                criteria nvarchar(1000) not null,
                table_name varchar(200) not null,
                processed bit not null,
                delete_sql varchar(1000)
        )

        insert into @to_delete (criteria, table_name, processed) values (@base_criteria, @base_table_name, 0)

        declare @id int, @criteria nvarchar(1000), @table_name varchar(200)
        while exists(select 1 from @to_delete where processed = 0) begin
                select top 1 @id = id, @criteria = criteria, @table_name = table_name from @to_delete where processed = 0 order by id desc

                insert into @to_delete (criteria, table_name, processed)
                        select referencing_column.name + ' in (select [' + referenced_column.name + '] from [' + @table_name +'] where ' + @criteria + ')',
                                referencing_table.name,
                                0
                        from  sys.foreign_key_columns fk
                                inner join sys.columns referencing_column on fk.parent_object_id = referencing_column.object_id 
                                        and fk.parent_column_id = referencing_column.column_id 
                                inner join  sys.columns referenced_column on fk.referenced_object_id = referenced_column.object_id 
                                        and fk.referenced_column_id = referenced_column.column_id 
                                inner join  sys.objects referencing_table on fk.parent_object_id = referencing_table.object_id 
                                inner join  sys.objects referenced_table on fk.referenced_object_id = referenced_table.object_id 
                                inner join  sys.objects constraint_object on fk.constraint_object_id = constraint_object.object_id
                        where referenced_table.name = @table_name
                                and referencing_table.name != referenced_table.name

                update @to_delete set
                        processed = 1
                where id = @id
        end

        select 'print ''deleting from ' + table_name + '...''; delete from [' + table_name + '] where ' + criteria from @to_delete order by id desc
end

exec usp_delete_cascade 'root_table_name', 'id = 123'
