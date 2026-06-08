import { Search } from 'lucide-react';
import styles from '../notes_search/notes_search.module.css';

function NotesSearch({query, setQuery}){
    return(
        <div className={styles.notes_search}>
            <Search size={18} color='#b4b4b4' style={{
                position: 'absolute',
                left: 10
            }}/>
            <input 
                placeholder='Поиск заметок'
                value={query}
                onChange={(e) => setQuery(e.target.value)}
            />
        </div>
    )
}

export default NotesSearch;