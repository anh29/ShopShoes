const {cnn, sql} = require('../../connect'); 
module.exports = function() {
    this.getAll = async function(result) {
        let pool = await cnn;
        let sqlString = "select * from AspNetUsers as users inner join AspNetUserRoles as userRoles on users.Id = userRoles.UserId where userRoles.RoleId = 3";
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

    // this.udpate = async function(newData, result) {
    //     let pool = await cnn;
    //     let sqlString = "Update tb_HangHoa SET Name = @name, Gender = @gender, DTB = @dtb, ID_Lop = @id_lop where MSSV = @id";
    //     return await pool.request()
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