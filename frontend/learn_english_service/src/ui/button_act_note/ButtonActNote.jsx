import { Edit, Edit2, Eye, Trash2 } from 'lucide-react';
import styles from '../button_act_note/button_act_note.module.css';

function ButtonActNote({type, onClick}){
    return(
        <div className={`${styles.button_act} ${styles[type]}`} onClick={onClick}>
            {type === "view" && <Eye size={16} color='#e0ab6a'/>}
            {type === "edit" && <Edit size={16} color='#d66400'/>}
            {type === "delete" && <Trash2 size={16} color='#cb0000'/>}
        </div>
    )
}

export default ButtonActNote;