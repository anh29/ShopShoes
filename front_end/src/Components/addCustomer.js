import react from 'react';
import Axios from 'axios';
import ReactDOM from 'react-dom/client';
import Warning from './warningBox';
const addCustomer = () => {        
    const [customer, setCustomer] = react.useState([]);
    react.useEffect(()=>{
        const loadCustomer = async() => {
            const repsonse = await Axios.get("http://localhost:4000/customer");
            setCustomer(repsonse.data.result);
        }
        loadCustomer();
    }, []);

    function Add() {
        let nameEle = document.getElementById('Name');
        let birthEle = document.getElementById('birth');
        let number_phoneEle = document.getElementById('number_phone');
        let addressEle = document.getElementById('address');    
        let emailEle = document.getElementById('Email');    
        const now = new Date();
        const time_create = `${now.getMonth() + 1}/${now.getDate()}/${now.getFullYear()} ${now.getHours()}:${now.getMinutes()}:${now.getSeconds()}`;
        let genderEle = document.getElementsByName('gender');
        let gender;
        for (let i = 0; i < genderEle.length; i++) {
            if (genderEle[i].checked) {
                gender = genderEle[i].value;
                break;
            }
        }
        let warningBox = ReactDOM.createRoot(document.getElementById('warning'));
        if(number_phoneEle.value !== '' || nameEle.value !== '') {
            console.log(number_phoneEle.value);
            let check = customer.some((cus) => {
                return cus.SDT === number_phoneEle.value;
            });
            if(check === false) {
                let object = {
                    IDRole: 1,
                    Name: nameEle.value,
                    Gender: gender,
                    Birth: birthEle.value,
                    phoneNumber: number_phoneEle.value,
                    accumulatedPoints: 0,
                    Address: addressEle.value,
                    Email: emailEle.value,
                    createDay: time_create
                }
                console.log(object);
                fetch('http://localhost:4000/customer', {
                    method: 'POST',
                    body: JSON.stringify(object),
                    headers: {
                        "Content-Type": "application/json"
                    }
                }).then((response) => {
                    response.json().then((res) => {
                        console.log(res.result);
                        if(res.result !== '') {
                            let customerEle = document.querySelector(".input-box-customer");
                            customerEle.id = res.result;
                            customerEle.value = nameEle.value + "-" + number_phoneEle.value;
                        }
                    });
                });
                const modal = document.querySelector(".modal");
                modal.classList.remove('open');        
            }else {
                warningBox.render(
                    <react.StrictMode>
                        <Warning content='Số điện thoại đã tồn tại !!!' name_class='warningBox'></Warning>
                    </react.StrictMode>
                )
            }
        }else {
            warningBox.render(
                <react.StrictMode>
                    <Warning content='Vui lòng điền tên và sô điện thoại !!!' name_class='warningBox'></Warning>
                </react.StrictMode>
            )
        }
    }

    function Cancel() {
        const modal = document.querySelector(".modal");
        modal.classList.remove('open');        
        document.getElementById('Name').value = '';
        document.getElementById('birth').value = '';
        document.getElementById('number_phone').value = '';
        document.getElementById('address').value = '';    
        document.getElementById('Email').value = '';    
    }
    return (
        <div>            
            <button id="OK" onClick={() => Add()}><i className="fa-solid fa-check"></i> OK</button>
            <button id="Cancel" onClick={() => Cancel()}><i className="fa-solid fa-xmark"></i> CANCEL</button>
        </div>
    );
}

export default addCustomer;