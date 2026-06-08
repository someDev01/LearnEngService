import { Book } from 'lucide-react';
import styles from '../button_dictionary/button_dictionary.module.css';

function ButtonDictionary({onClick}){
    return(
        <div className={styles.button} onClick={onClick}>
            <Book size={20} color='#c4c4c4'/>
            <div className={styles.dictionary}>Личный словарь</div>
        </div>
    )
}

export default ButtonDictionary;