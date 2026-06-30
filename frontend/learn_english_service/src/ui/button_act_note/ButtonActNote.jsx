import { Edit2, Eye, Trash2 } from 'lucide-react';
import styles from '../button_act_note/button_act_note.module.css';

function ButtonActNote({type, onClick}){
    return(
        <div className={`${styles.button_act} ${styles[type]}`} onClick={onClick}>
            {type === "view" && <Eye size={16} color='#edb673'/>}
            {type === "edit" && <Edit2 size={16} color='#ec6e00'/>}
            {type === "delete" && <Trash2 size={16} color='#e00000'/>}
        </div>
    )
}

export default ButtonActNote;