import { useEffect, useState } from 'react';
import Modal from '../../modal/Modal';
import ButtonClose from '../../ui/button_close/ButtonClose';
import ButtonSaveNote from '../../ui/button_save_note/ButtonSaveNote';
import EditNoteInput from '../../ui/edit_note_input/EditNoteInput';
import styles from '../edit_note/edit_note_form.module.css';
import { noteApi } from '../../api/note';
import { toast } from 'react-toastify';
import { closeModalEditNote } from '../../redux/slices/modalSlice';
import ErrorText from '../../ui/error/ErrorText';
import ButtonX from '../../ui/button_x/ButtonX';

function EditNoteForm({isOpen, onClose, onCreateNote, onUpdateNote, note, isEditing, loadingSave}){

    const [wordInput, setWordInput] = useState('');
    const [wordInputError, setWordInputError] = useState(null);
    const [translationsInput, setTranslationsInput] = useState('');
    const [examplesInput, setExamplesInput] = useState([
        {text: '', translate: ''}
    ]);

    const onWordInputChange = (e) => {
        const value = e.target.value;
        setWordInput(value);
    }
    const onTranslationsInputChange = (e) => {
        const value = e.target.value;
        setTranslationsInput(value);     
    }
    const onExamplesInputChange = (index, field, value) => {
        setExamplesInput(prev =>
            prev.map((item, i) =>
                i === index
                    ? { ...item, [field]: value }
                    : item
            )
        );
    }

    useEffect(() => {
        if(note){
            setWordInput(note.word || '');
            setTranslationsInput(note.translations?.join(', ') || '');
            setExamplesInput(note.examples || [{text: '', translate: ''}]);
        }
        else{
            setWordInput('');
            setTranslationsInput('');
            setExamplesInput([{text: '', translate: ''}]);
        }
    }, [note]);

    const handleSave = () => {
        if(!wordInput.trim()){
            setWordInputError("заполните поле");
            return;
        }
        const translationsArray = translationsInput.split(', ');
        const examplesArray = examplesInput.filter(ex => ex.text.trim() && ex.translate.trim());    
        
        if(isEditing){
            onUpdateNote(note.id, wordInput, translationsArray, examplesArray);
        }
        else{
            onCreateNote(wordInput, translationsArray, examplesArray)
        }
    }

    return(
        <Modal isOpen={isOpen}>
            <div className={styles.edit__container}>
                <div className={styles.header_form}>
                    {isEditing ? <p>Редактировать слово</p> : <p>Добавить слово</p>}
                    <ButtonX onClick={onClose}/>
                </div>
                <div className={styles.inputs_part}>
                    <EditNoteInput 
                        text="СЛОВО (EN/RU)" 
                        value={wordInput}
                        onInputChange={onWordInputChange}
                        placeholder="word" 
                        maxLength={25}
                    />
                    {wordInputError && <ErrorText>{wordInputError}</ErrorText>}
                    <EditNoteInput 
                        text="ПЕРЕВОД (Ы)" 
                        value={translationsInput}
                        onInputChange={onTranslationsInputChange}
                        placeholder="(необязательно)" 
                        maxLength={50}
                    />
                    {examplesInput.map((ex, index) => (
                        <div key={index}>
                            <EditNoteInput
                                text='ПРИМЕР EN' 
                                value={ex.text}
                                onInputChange={(e) => onExamplesInputChange(index, 'text', e.target.value)}
                                placeholder="(необязательно)" 
                                maxLength={100}
                            />
                            <EditNoteInput
                                text='ПРИМЕР RU' 
                                value={ex.translate}
                                onInputChange={(e) => onExamplesInputChange(index, 'translate', e.target.value)}
                                placeholder="(необязательно)" 
                                maxLength={100}
                            />
                        </div>
                    ))}
                </div>
                <ButtonSaveNote onClick={handleSave} loadingSave={loadingSave}/>
            </div>
        </Modal>
    )
}

export default EditNoteForm;