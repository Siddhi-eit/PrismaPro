import React, { useEffect, useState } from 'react';
import { connect } from 'react-redux';
import { addFormula, getFormulaList } from 'redux/actions';
import FormulaListing from 'containers/formula/FormulaList';
import FormulaListPageHeading from 'containers/pages/FormulaListPageHeading';
import createNotification from 'helpers/alerts';
import FormulaModal from 'containers/pages/FormulaModal';
import DeleteModal from 'containers/pages/DeleteModal';

import {
  addFormulaResetState,
  deleteFormula,
  getFormulaByID,
  downloadFormulaExcelFile,
  getCanisterLookupByCanisterCode,
  getCanisterList,
} from 'redux/actions';

const orderOptions = [
  { column: '1', label: 'ProductCode' },
  { column: '2', label: 'Amounts' },
];
const orderDirection = [
  { sortOrder: 'asc', label: 'A-Z' },
  { sortOrder: 'desc', label: 'Z-A' },
];

const getIndex = (value, arr, prop) => {
  for (let i = 0; i < arr.length; i += 1) {
    if (arr[i][prop] === value) {
      return i;
    }
  }
  return -1;
};

const pageSizes = [10, 20, 50, 100];

const FormulaList = ({
  match,
  allFormulaItems,
  FormulaListAction,
  FormulaDeleteAction,
  isSucessfullyDelete,
  FormulaAddAction,
  isSucessfullyAdd,
  AddFormulaResetStateAction,
  FormulaGetByIDAction,
  formulaDetail,
  loading,
  formulaID,
  resultMessage,
  DownloadFormulaExcelFileAction,
  lookupData,
  OnGetCanisterLookup,
}) => {
  const [isLoaded, setIsLoaded] = useState(false);
  const [displayMode, setDisplayMode] = useState('thumblist');
  const [currentPage, setCurrentPage] = useState(1);
  const [selectedPageSize, setSelectedPageSize] = useState(10);
  const [search, setSearch] = useState('');
  const [selectedOrderOption, setSelectedOrderOption] = useState({
    column: '1',
    label: 'ProductCode',
  });
  const [modalOpen, setModalOpen] = useState(false);
  const [totalItemCount, setTotalItemCount] = useState(0);
  const [totalPage, setTotalPage] = useState(1);
  const [selectedItems, setSelectedItems] = useState([]);
  const [selectedFormulaID, setSelectedFormulaID] = useState('');
  const [items, setItems] = useState([]);
  const [lastChecked, setLastChecked] = useState(null);
  const [deleteMessage, setDeleteMessage] = useState(null);
  const [isDeleteModalOpen, seIsDeleteModalOpen] = useState(false);
  const [selectedOrderDirection, setSelectedOrderDirection] = useState({
    sortOrder: 'desc',
    label: 'Z-A',
  });

  useEffect(() => {
    setCurrentPage(1);
  }, [selectedPageSize, selectedOrderOption]);

  useEffect(() => {
    const fetchData = () => {
      const gridObject = {
        pageSize: selectedPageSize,
        currentPage,
        orderBy: selectedOrderOption.column,
        search,
        orderDirection: selectedOrderDirection.sortOrder,
        formulaID: formulaID,
      };
      FormulaListAction(gridObject);
    };
    fetchData();
  }, [
    selectedPageSize,
    currentPage,
    selectedOrderOption,
    search,
    formulaID,
    selectedOrderDirection,
  ]);

  useEffect(() => {
    if (isSucessfullyAdd != null && isSucessfullyAdd > 0) {
      setModalOpen(!modalOpen);
      AddFormulaResetStateAction();
    }
    if (allFormulaItems != null) {
      setTotalPage(allFormulaItems.totalPage);
      setItems(
        allFormulaItems.data.map((x) => {
          return { ...x };
        })
      );
      setSelectedItems([]);
      setTotalItemCount(allFormulaItems.totalItem);
      setIsLoaded(true);
    }
  }, [
    allFormulaItems,
    selectedPageSize,
    currentPage,
    selectedOrderOption,
    search,
    isSucessfullyAdd,
    selectedOrderDirection,
  ]);

  useEffect(() => {
    if (
      (isSucessfullyDelete !== null &&
        isSucessfullyDelete !== undefined &&
        isSucessfullyDelete !== 0) ||
      (isSucessfullyAdd != null && isSucessfullyAdd > 0)
    ) {
      const gridObject = {
        pageSize: selectedPageSize,
        currentPage,
        orderBy: selectedOrderOption.column,
        search,
        orderDirection: selectedOrderDirection.sortOrder,
        formulaID: formulaID,
      };
      FormulaListAction(gridObject);
    }
    if (isSucessfullyAdd != null && isSucessfullyAdd > 0) {
      createNotification('success', 'Success', resultMessage);
    }
    if (isSucessfullyAdd != null && isSucessfullyAdd === -1) {
      createNotification('warning', 'Warning', resultMessage);
    }
    if (isSucessfullyAdd != null && isSucessfullyAdd === -2) {
      createNotification('warning', 'Warning', resultMessage);
    }
    if (isSucessfullyDelete !== null && isSucessfullyDelete > 0) {
      createNotification('success', 'Success', 'Deleted Successfully !!');
    }
  }, [isSucessfullyDelete, isSucessfullyAdd]);

  const onCheckItem = (event, id) => {
    if (
      event.target.tagName === 'A' ||
      (event.target.parentElement && event.target.parentElement.tagName === 'A')
    ) {
      return true;
    }
    if (lastChecked === null) {
      setLastChecked(id);
    }
    let selectedList = [...selectedItems];
    if (selectedList.includes(id)) {
      selectedList = selectedList.filter((x) => x !== id);
    } else {
      selectedList.push(id);
    }
    setSelectedItems(selectedList);
    if (event.shiftKey) {
      let newItems = [...items];
      const start = getIndex(id, newItems, 'id');
      const end = getIndex(lastChecked, newItems, 'id');
      newItems = newItems.slice(Math.min(start, end), Math.max(start, end) + 1);
      selectedItems.push(
        ...newItems.map((item) => {
          return item.id;
        })
      );
      selectedList = Array.from(new Set(selectedItems));
      setSelectedItems(selectedList);
    }
    document.activeElement.blur();
    return false;
  };

  const onContextMenuClick = (e, data) => {
    if (data.action === 'delete') {
      setDeleteMessage('Are you sure you want to delete?');
      seIsDeleteModalOpen(!isDeleteModalOpen);
      AddFormulaResetStateAction();
    } else if (data.action === 'Edit') {
      FormulaGetByIDAction(selectedFormulaID);
      OnGetCanisterLookup();
    }
  };

  useEffect(() => {
    if (formulaDetail != null) {
      setModalOpen(!modalOpen);
    }
  }, [formulaDetail]);
  const onContextMenu = (e, data) => {
    const clickedProductId = data.data;
    setSelectedFormulaID(clickedProductId);
    if (!selectedItems.includes(clickedProductId)) {
      setSelectedItems([clickedProductId]);
    }
    return true;
  };

  const startIndex = (currentPage - 1) * selectedPageSize;
  const endIndex = currentPage * selectedPageSize;

  const onDeleteClick = () => {
    seIsDeleteModalOpen(!isDeleteModalOpen);
    FormulaDeleteAction(selectedFormulaID);
  };
  const onFormulaSubmit = (value) => {
    FormulaAddAction(value);
  };

  const ExportFormulaData = () => {
    DownloadFormulaExcelFileAction();
  };

  const handleSearchKey = (e) => {
    if (e.key === 'Enter') {
      setSearch(e.target.value.toLowerCase());
    }
  };

  return (
    <>
      <div>
        {loading && (
          <div className="customOverlay">
            <div className="loading" />
          </div>
        )}
        <FormulaListPageHeading
          displayMode={displayMode}
          heading="Formulas"
          changeDisplayMode={setDisplayMode}
          changePageSize={setSelectedPageSize}
          onSearchKey={handleSearchKey}
          changeOrderBy={(column) => {
            setSelectedOrderOption(
              orderOptions.find((x) => x.column === column)
            );
          }}
          selectedPageSize={selectedPageSize}
          totalItemCount={totalItemCount}
          selectedOrderOption={selectedOrderOption}
          match={match}
          startIndex={startIndex}
          endIndex={endIndex}
          selectedItemsLength={selectedItems ? selectedItems.length : 0}
          itemsLength={items ? items.length : 0}
          orderOptions={orderOptions}
          pageSizes={pageSizes}
          toggleModal={() => {
            OnGetCanisterLookup();
            setModalOpen(!modalOpen);
            AddFormulaResetStateAction();
          }}
          orderDirection={orderDirection}
          selectedOrderDirection={selectedOrderDirection}
          changeOrderDirection={(sortOrder) => {
            setSelectedOrderDirection(
              orderDirection.find((x) => x.sortOrder === sortOrder)
            );
          }}
          ExportData={() => {
            ExportFormulaData();
          }}
        />
        <FormulaModal
          modalOpen={modalOpen}
          toggleModal={() => {
            setModalOpen(!modalOpen);
            AddFormulaResetStateAction();
            OnGetCanisterLookup();
          }}
          onFormulaSubmit={onFormulaSubmit}
          formulaDetail={formulaDetail}
          canisterLookupData={lookupData}
        />
        <FormulaListing
          items={items}
          allFormulaItems={allFormulaItems}
          displayMode={displayMode}
          selectedItems={selectedItems}
          onCheckItem={onCheckItem}
          currentPage={currentPage}
          totalPage={totalPage}
          onContextMenuClick={onContextMenuClick}
          onContextMenu={onContextMenu}
          onChangePage={setCurrentPage}
        />
        <DeleteModal
          deleteMessage={deleteMessage}
          isDeleteModalOpen={isDeleteModalOpen}
          deletedClick={() => onDeleteClick()}
          toggleModal={() => seIsDeleteModalOpen(!isDeleteModalOpen)}
        />
      </div>
    </>
  );
};

const mapStateToProps = ({ formula, canister, user }) => {
  const {
    allFormulaItems,
    loading,
    formulaDetail,
    isSucessfullyAdd,
    isAddError,
    resultMessage,
    isSucessfullyDelete,
  } = formula;
  const { lookupData } = canister;
  const { formulaID } = user;
  return {
    allFormulaItems,
    loading,
    lookupData,
    formulaID,
    formulaDetail,
    isSucessfullyAdd,
    isAddError,
    resultMessage,
    isSucessfullyDelete,
  };
};

export default connect(mapStateToProps, {
  FormulaDeleteAction: deleteFormula,
  FormulaListAction: getFormulaList,
  FormulaAddAction: addFormula,
  AddFormulaResetStateAction: addFormulaResetState,
  FormulaGetByIDAction: getFormulaByID,
  OnGetCanisterLookup: getCanisterLookupByCanisterCode,
  DownloadFormulaExcelFileAction: downloadFormulaExcelFile,
})(FormulaList);
