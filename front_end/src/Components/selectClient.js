import react from 'react'
import Axios from 'axios'
//import fs from 'fs';

const selectClient = () => { 
    const [client, setClient] = react.useState([]);
    const [suggestion, setSuggestion] = react.useState([]);
    const [text, setText] = react.useState('');
    const [id, setId] = react.useState('');
    react.useEffect(()=>{
        async function loadProduct() {
            const repsonse = await Axios.get("http://localhost:4000/client");
            setClient(repsonse.data.result);
        }
        loadProduct();
        setId("0")
    }, []);
    function onChangeHandler(text) {
        let result = [];
        if (text.length > 0) {
            result = client.filter((keyword) => {
                return (keyword.Name.toLowerCase().includes(text.toLowerCase()) === true || keyword.SDT.includes(text) === true);
            });
        }
        else setId("0");
        setSuggestion(result);
        setText(text);  
    }

    function selectClient(client) {
        setText(client.Name + '-' + client.SDT);
        setId(client.ID);
        setSuggestion([]);  
        // const jsonData = JSON.stringify();
        // fs.writeFile('./bill.json', jsonData, (err) => {
        //   if (err) throw err;
        //   console.log('Saved!');
        // });
    }
    return (            
        <div className="find-client">
            <input type="text" id={id} className="input-box-client" style={{paddingLeft: '40px'}} placeholder="Tìm nhân viên" onChange={e =>onChangeHandler(e.target.value)} value={text} />
            <span href="true"><i className="icon-find-client fa-solid fa-magnifying-glass" /></span>
            <div className="result-box-client">                        
                <ul>
                    {suggestion.map((client) =>
                        <li key={client.ID} className='extra-client' onClick = {() => selectClient(client)}>{client.Name} - {client.SDT}</li>
                    )}
                </ul>
            </div>
      </div>
    );
} 
export default selectClient