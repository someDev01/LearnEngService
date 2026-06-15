import { useParams } from "react-router-dom";
import styles from '../content/content_page.module.css';
import ButtonBack from "../../ui/button_back/ButtonBack";
import DescriptionContent from "../../ui/description_content/DescriptionContent";
import Video from "../../ui/video/Video";
import { useEffect, useState } from "react";
import { videoApi } from "../../api/video";
import { toast } from "react-toastify";
import { useDispatch, useSelector } from "react-redux";
import VideosSkeleton from "../../ui/videos_skeleton/VideosSkeleton";
import VideoPlayer from "../../widgets/video_player/VideoPlayer";
import SearchVideo from "../../ui/search/SearchVideo";
import { GetPageSize } from "../../utils/size/getPageSize";
import { useSearch } from "../../hooks/search/useSearch";
import LoadMoreButton from "../../ui/button_load_more/LoadMoreButton";
import LoaderSearch from "../../ui/loader_search/LoaderSearch";
import NoFound from "../../ui/nofound/NoFound";
import VpnWarming from "../../ui/vpn_warming/VpnWarming";

const timeoutVideos = 500;

function ContentPage(){

    const isOpen = useSelector(state => state.modal.isOpenVideoModal);    
    const openedVideo = useSelector(state => state.modal.openedVideo);

    const dispatch = useDispatch();
    const [data, setData] = useState(null);   
    const [loading, setLoading] = useState(false);

    const [page, setPage] = useState(1);
    const pageSize = GetPageSize();
    const [hasMore, setHasMore] = useState(false);

    const [searchPage, setSearchPage] = useState(1);

    const {query, setQuery, searchResults, setSearchResults, isLoading, setIsLoading, hasSearched, hasMore: searchHasMore, setHasMore: setSearchHasMore} 
    = useSearch({
        fetchFunction: (query, page) => videoApi.searchVideos(query, page, pageSize), 
        extractData: (response) => response.data.data
    });

    const [showWarming, setShowWarming] = useState(true);

    const loadMore = async() => {
        const nextPage = page + 1;
        setPage(nextPage);

        const response = await videoApi.getVideos(nextPage, pageSize);

        if(response.success){
            setData(prev => [
                ...prev,
                ...response.data.data
            ]);
        }

        else{
            toast.error("Ошибка получения заметок");
            setNotes([]);
        }

        setHasMore(response?.data.page < response?.data.totalPages);
    };

    const loadMoreSearch = async() => {
        const nextPage = searchPage + 1;
        setSearchPage(nextPage);

        const response = await videoApi.searchVideos(query, nextPage, pageSize);

        if(response.success){
            setSearchResults(prev => [
                ...prev, 
                ...response.data.data
            ]);

            setSearchHasMore(response.data.page < response.data.totalPages);
        }
        else{
            toast.error("Ошибка получения заметок");
            setSearchResults([]);
        }
    };

    useEffect(() => {
        setSearchPage(1);
        setSearchResults([]);
        setSearchHasMore(false);
    }, [query.trim()]);

    useEffect(() => {  
        setLoading(true);
        
        const timeout = setTimeout(() => {
            
            const fetchVideos = async() => {       
                const response = await videoApi.getVideos(page, pageSize);
            
                if(response.success){
                    const data = response.data;
                    setData(data.data);
                }
                else{            
                    toast.error(response.error);
                    setLoading(false);
                    return;
                }

                setLoading(false);
                setHasMore(response.data.page < response.data.totalPages);
            }
            fetchVideos();

        }, timeoutVideos);

        return () => clearTimeout(timeout);

    }, [dispatch]);
    
    const displayVideos = query.trim().length >= 2 ? searchResults: data;

    return(
        <>
            <div className={styles.content__container}>
                <div className={styles.content}>
                    <div className={styles.top_part}>
                        <ButtonBack/>
                        {showWarming && (<VpnWarming onClick={() => showWarming(false)}/>)}
                        <SearchVideo
                            query={query}
                            setQuery={setQuery}
                        />
                    </div>
                    <div className={styles.bottom_part}>
                        {isLoading && (
                            <div className={styles.wrapper_loader} style={{display: 'flex', justifyContent: 'center', alignItems: 'center', width: '100%'}}>
                                <LoaderSearch/>
                            </div>
                        )}
                        <div className={styles.list_videos}>
                            {!loading && !isLoading && displayVideos && displayVideos.map((item) => (
                                <Video 
                                    key={item.id}
                                    video={item}
                                    youtubeId={item.youtubeId}
                                />
                            ))}
                            {loading && (<VideosSkeleton/>)}
                        </div>
                        {!loading && !isLoading && query.trim().length < 2 && hasMore && (
                            <LoadMoreButton onClick={loadMore}/>
                        )}
                        {!loading && !isLoading && query.trim().length >= 2 && searchHasMore && (
                            <LoadMoreButton onClick={loadMoreSearch}/>
                        )}
                        {!loading && !isLoading && query.trim().length >=2 && hasSearched && searchResults.length === 0  
                            && (<div className="" style={{display: 'flex', justifyContent:'center', alignItems:'center', width:'100%'}}>
                                    <NoFound/>
                                </div>)
                        }
                    </div>
                </div>
            </div>
            {isOpen && openedVideo && <VideoPlayer
                isOpen={isOpen}
                videoId={openedVideo?.id}
                selectedYoutubeId={openedVideo.youtubeId}
            />}
        </>
    )
}

export default ContentPage;