const {cnn, sql} = require('../../connect'); 
module.exports = function() {
    this.getAll = async function(result) {
        let pool = await cnn;
        let sqlString = "select * from AspNetUsers as users inner join AspNetUserRoles as userRoles on users.Id = userRoles.UserId where userRoles.RoleId = (select Id from AspNetRoles where Name = 'Customer')";
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
    //     let sqlString = "Select * from tb_KhachHang where IDKhachHang like '%' + @varId + '%'";
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

    this.create = async function(newData, result) {
        let pool = await cnn;
        let sqlString = `insert into AspNetUsers (Id, FullName, PhoneNumber, UserName, EmailConfirmed, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnabled, AccessFailedCount, PasswordHash, accumulatedPoints, Address, Email) 
                                            values (@PhoneNumber, @FullName, @PhoneNumber, @PhoneNumber, 'false', 'true', 'false', 'false', 0, '123456', @accumulatedPoints, @Address, @Email) 
                        insert into AspNetUserRoles(UserId, RoleId) values (@PhoneNumber, (select Id from AspNetRoles where Name = 'Customer'))`;
        return await pool.request()
        .input('FullName', sql.NVarChar, newData.FullName)
        .input('PhoneNumber', sql.NVarChar, newData.PhoneNumber)
        .input('Address', sql.NVarChar, newData.Address)
        .input('accumulatedPoints', sql.Float, newData.accumulatedPoints)
        .input('Email', sql.NVarChar, newData.Email)
        .query(sqlString, function(err, data) {
            if(err) {
                result(true, null);
            }else {
                result(null, data);
            }
        });
    };

    this.udpateCore = async function(newData, result) {
        let pool = await cnn;
        let sqlString = "Update AspNetUsers SET accumulatedPoints = accumulatedPoints + @DiemTang where Id = @id";
        return await pool.request()
        .input('DiemTang', sql.Float, newData.DiemTang)
        .input('id', sql.NVarChar, newData.Id)
        .query(sqlString, function(err, data) {
            if(err) {
                result("Có lỗi xảy ra!!!", null);
            }else {
                result(null, "Thành công");
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