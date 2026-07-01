import { useEffect, useState } from "react";
import Modal from "../../modal/Modal";
import ButtonAddNote from "../../ui/button_add/ButtonAddNote";
import styles from '../popup/word_popup.module.css';
import { translateApi } from "../../api/translate";
import { toast } from "react-toastify";
import ButtonAdded from "../../ui/button_added/ButtonAdded";
import { Check, X } from "lucide-react";
import ButtonX from "../../ui/button_x/ButtonX";

function WordPopUp({isOpen, word, onAddNoteWithContext, onClose, noteCreating, setWord, onHasWordInNotes}){

    const [loading, setLoading] = useState(false);
    const [translation, setTranslation] = useState('показать перевод');
    const [isShowedTranslation, setIsShowedTranslation] = useState(false);

    const handleShowTranslation = async(word) => {
        if(isShowedTranslation) {
            return;
        };

        setLoading(true);
        const response = await translateApi.showTranslation(word);

        if(response.success){
            setTranslation(response.data);
        }
        else{
            toast.error(response.error);
        }
        setLoading(false);
        setIsShowedTranslation(true);
    }

    useEffect(() => {
        if(!isOpen){
            setTranslation('показать перевод')
            setIsShowedTranslation(false);
            setLoading(false);
        }
    }, [isOpen])

    return(
        <Modal isOpen={isOpen}>
            <div className={styles.popup}>
                <div className={styles.header_part}>
                    <input
                        className={styles.word_input}
                        value={word}
                        maxLength={25}
                        onChange={(e) => {setWord(e.target.value)}} 
                    />
                    <ButtonX onClick={onClose}/>
                </div>
                <div className={styles.translation_part} onClick={() => handleShowTranslation(word)}>
                    {loading ? <p>загрузка...</p> : <p>{translation}</p>}
                </div>
                {word && onHasWordInNotes(word) ? 
                    <ButtonAdded text="Добавлено"/> :
                    <ButtonAddNote 
                        selectedText={word}
                        onAddWordWithContext={onAddNoteWithContext}
                        noteCreating={noteCreating}
                    />}
            </div>
        </Modal>
    )
}

export default WordPopUp;