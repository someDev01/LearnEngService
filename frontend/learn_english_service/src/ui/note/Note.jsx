import ButtonActNote from '../button_act_note/ButtonActNote';
import CreatedAtNote from '../createdat_note/CreatedAtNote';
import WordLvl from '../lvl/WordLvl';
import styles from '../note/note.module.css';
import Transcription from '../transcription/Transcription';
import Translations from '../translations/Translations';
import Word from '../word/Word';


function Note({word, translations = [], transcription, lvl, createdAt, onOpenViewNote, onOpenEditNote, onDeleteNote}){

    return(
        <div className={styles.card}>
            <div className={styles.top_part}>
                <div className={styles.header_note}>
                    <Word word={word}/>
                    <WordLvl lvl={lvl}/>
                </div>
                <div className={styles.info_note}>
                    <Transcription 
                        word={word}
                        transcription={transcription}
                    />
                    <Translations translations={translations} size={16}/>
                </div>
            </div>
            <div className={styles.bottom_part}>
                <div className={styles.block_buttons}>
                    <ButtonActNote type="view" onClick={onOpenViewNote}/>
                    <ButtonActNote type="edit" onClick={onOpenEditNote}/>
                    <ButtonActNote type="delete" onClick={onDeleteNote}/>
                </div>
                <CreatedAtNote createdAt={createdAt}/>
            </div>
        </div>
    )
}

export default Note;