import { Trash2 } from 'lucide-react';
import styles from '../act_context_button/act_context_button.module.css';

function ActContextButton({type, onClick}){
    return(
        <div className={`${styles.act_button} ${styles[type]}`} onClick={onClick}>
            {type === "delete" && <Trash2 size={18} color='#c30000'/>}
        </div>
    )
}
export default ActContextButton;