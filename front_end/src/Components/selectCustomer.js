import react from 'react'
import Axios from 'axios'

const selectCustomer = () => {         
    // const [product, setProduct] = useState([]);
    // const [text, setText] = useState('');
    // useEffect(()=>{
    //     const loadProduct = async() => {
    //         const repsonse = await Axios.get("http://localhost:4000/product");
    //         setProduct(repsonse.data.result);
    //     }
    //     loadProduct();
    // }, []);
    // const onChangeHandler = (text) => {
    //     let matches = [];
    //     if(text.length > 0) {
    //         matches = product.filter(u => {
    //             const regex = new RegExp(`${text}`);
    //             return u.TenHang.match(regex);
    //         });
    //     }
    //     console.log(matches);
    //     setsuggestion(matches);
    //     setText(text);
    // }
    // const resultsBox = document.querySelector(".result-box");
    // const inputBox = document.getElementById("input-box"); 
    const [product, setProduct] = react.useState([]);
    const [suggestion, setSuggestion] = react.useState([]);
    const [text, setText] = react.useState('');
    react.useEffect(()=>{
        const loadProduct = async() => {
            const repsonse = await Axios.get("http://localhost:4000/customer");
            setProduct(repsonse.data.result);
        }
        loadProduct();
    }, []);
    // const resultsBox = document.querySelector(".result-box-client");
    // const inputBox = document.getElementById("input-box-client");

    const onChangeHandler = (text) => {
        let result = [];
        if(text.length > 0) {
            result = product.filter((keyword) => {
                return (keyword.Name.toLowerCase().includes(text.toLowerCase()) === true || keyword.SDT.includes(text) === true);
            });
            console.log(result);
        }
        console.log(result);
        setSuggestion(result);
        setText(text);
    }

    const selectCustomer = (customer) => {
        setText(customer.Name + "-" + customer.SDT);
        setSuggestion([]);
    }
    // function display(result) {
    //     const content = result.map((list, i)=> {
    //         return `<li key = ${i} onclick = {selectProduct()}> ${list.Name}  - ${list.SDT} </li>`;
    //     });
    //     resultsBox.innerHTML = "<ul>" + content.join(' ') +"</ul>";
    // }
    return (            
        <div className="find-customer">
        <input type="text" className="input-box-customer" style={{paddingLeft: '40px'}} placeholder="Tìm khách hàng" onChange={e =>onChangeHandler(e.target.value)} value={text} />
        <span href="true"><i className="icon-find-customer fa-solid fa-magnifying-glass" /></span>
        <div className="result-box-customer">                        
            <ul>
                {suggestion.map((customer) =>
                    <li key={customer.IDKhachHang} className='extra-customer' onClick = {() => selectCustomer(customer)}>{customer.Name} - {customer.SDT}</li>
                )}
            </ul>
        </div>
      </div>
    );
} 
export default selectCustomer