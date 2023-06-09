const {cnn, sql} = require('../../connect'); 
module.exports = function() {
    this.getAll = async function(result) {
        let pool = await cnn;
        let sqlString = "select p.Id, p.Title, p.Quantity, p.Price, tb_ProductImage.Image from tb_Product as p inner join tb_ProductImage on p.Id = tb_ProductImage.Id ";
        return await pool.request().query(sqlString, function(err, data) {
            if(data.recordset.length > 0)  {
                result(null, data.recordset);
            }
            else {
                result(null, err);
            }
        });
    };

    // this.getOne = async function(id, result) {
    //     let pool = await cnn;
    //     let sqlString = "Select * from tb_HangHoa where TenHang like '%' + @varId + '%'";
    //     return await pool.request()
    //     .input('varId', sql.NVarChar, id)
    //     .query(sqlString, function(err, data) {
    //         if(data.recordset.length > 0)  {
    //             result(null, data.recordset[0]);
    //         }
    //         else {
    //             result(true, null);
    //         }
    //     });
    // };

    // this.create = async function(newData, result) {
    //     let pool = await cnn;
    //     let sqlString = "Insert into tb_HangHoa(MSSV, Name, Gender, DTB, ID_Lop) values(@mssv, @name, @gender, @dtb, @id_lop)";
    //     return await pool.request()
    //     .input('mssv', sql.Int, newData.MSSV)
    //     .input('name', sql.NVarChar, newData.Name)
    //     .input('gender', sql.Bit, newData.Gender)
    //     .input('dtb', sql.Int, newData.DTB)
    //     .input('id_lop', sql.Int, newData.ID_Lop)
    //     .query(sqlString, function(err, data) {
    //         if(err) {
    //             result(true, null);
    //         }else {
    //             result(null, newData);
    //         }
    //     });
    // };

    this.update = async function(newData, result) {
        let pool = await cnn;
        let sqlString = "Update tb_Product SET Quantity = Quantity - @SoLuongBan where Id = @id";
        return await pool.request()
        .input('SoLuongBan', sql.Int, newData.SoLuongBan)
        .input('id', sql.Int, newData.Id)
        .query(sqlString, function(err, data) {
            if(err) {
                result(true, null);
            }else {
                result(null, newData);
            }
        });
    };
    
    // this.delete = async function(id, result) {
    //     let pool = await cnn;
    //     let sqlString = "Delete from tb_HangHoa where MSSV = @varId";
    //     return await pool.request().input('varId', sql.Int, id).query(sqlString, function(err, data) {
    //         if(!err) {
    //             res.send({result: "Da xoa thanh cong"});
    //         }
    //         else {
    //             res.send({result: "Xoa doi tuong that bai"});
    //         }
    //     });
    // };
}