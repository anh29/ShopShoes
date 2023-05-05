const {cnn, sql} = require('../../connect'); 
module.exports = function() {
    this.getAll = async function(result) {
        let pool = await cnn;
        let sqlString = "Select * from customer";
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
        let sqlString = "Insert into customer(IDRole, Name, Gender, Birth, phoneNumber, Address, accumulatedPoints, createDay, Email) output inserted.ID_Customer values(@IDRole, @Name, @Gender, @Birth, @phoneNumber, @Address,  @accumulatedPoints, @createDay, @Email)";
        return await pool.request()
        .input('IDRole', sql.Int, newData.IDRole)
        .input('Name', sql.NVarChar, newData.Name)
        .input('Gender', sql.NVarChar, newData.Gender)
        .input('Birth', sql.DateTime, newData.Birth)
        .input('phoneNumber', sql.NVarChar, newData.phoneNumber)
        .input('Address', sql.NVarChar, newData.Address)
        .input('accumulatedPoints', sql.Int, newData.accumulatedPoints)
        .input('createDay', sql.DateTime, newData.createDay)
        .input('Email', sql.NVarChar, newData.Email)
        .query(sqlString, function(err, data) {
            if(err) {
                result(true, null);
            }else {
                result(null, data.recordset[0].IDKhachHang);
            }
        });
    };

    this.udpateCore = async function(newData, result) {
        let pool = await cnn;
        let sqlString = "Update customer SET accumulatedPoints = accumulatedPoints + @DiemTang where ID_Customer = @id";
        return await pool.request()
        .input('DiemTang', sql.Int, newData.DiemTang)
        .input('id', sql.Int, newData.id)
        .query(sqlString, function(err, data) {
            if(err) {
                result(true, null);
            }else {
                result(null, "Đã tăng điểm tích luỹ thành công");
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