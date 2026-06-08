import { Check } from 'lucide-react';
import styles from '../button_added/button_added.module.css';

function ButtonAdded({text}){
    return(
        <div className={styles.button_added}>
            <Check size={18} color='#ffc694'/>
            <p>{text}</p>
        </div>
    )
}

export default ButtonAdded;