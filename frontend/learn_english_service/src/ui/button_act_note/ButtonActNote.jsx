import { Edit2, Eye, Trash2 } from 'lucide-react';
import styles from '../button_act_note/button_act_note.module.css';

function ButtonActNote({type, onClick}){
    return(
        <div className={`${styles.button_act} ${styles[type]}`} onClick={onClick}>
            {type === "view" && <p><Eye size={18} color='#ffc680'/></p>}
            {type === "edit" && <p><Edit2 size={18} color='#ff7700'/></p>}
            {type === "delete" && <p><Trash2 size={18} color='red'/></p>}
        </div>
    )
}

export default ButtonActNote;